Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Web.Script.Serialization
Imports System.Web
Imports VMware.Vim
Imports AV_Backup_Prep.AppVolumes
Imports System.Management.Automation
Imports System.Management.Automation.Runspaces
Imports System.Collections.ObjectModel
Imports System.Net.Security

Module Module1
    Public cookies As New CookieContainer
    Public theuri As Uri
    Public newuri As Uri
    Public ssession As String
    Public susername As String
    Public sserver As String
    Public spw As String
    Public svm As String
    Public awritableid As New ArrayList
    Public htwritables As New Hashtable
    Public adetachedwv As New ArrayList
    Public svc As String = ""
    Public adisks As New ArrayList
    Public awvinuse As New ArrayList
    Dim stype As String = ""
    Public awvtoattach As New ArrayList

    Sub Main()

        On Error Resume Next

        Console.WriteLine()
        '  Invoke this sample with an arbitrary set of command line arguments.
        Dim arguments As String() = Environment.GetCommandLineArgs()

        sserver = arguments(1)
        susername = arguments(2)
        spw = arguments(3)
        svc = arguments(4)
        svm = arguments(5)
        stype = arguments(6)

        If arguments.Count <> 7 Then

            Console.WriteLine("Usage: AV_Backup_Prep.exe ""App Volumes Manager"" ""App Volumes Username"" ""App Volumes User PW"" ""Virtual Center Name"" ""Backup VM Name"" ""PRE or POST")
            Console.ReadLine()

        End If

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls Or SecurityProtocolType.Ssl3 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls12

        Select Case Trim(UCase(stype))

            Case "PRE"
                Console.WriteLine("Running pre-backup preparation processes...")
                Console.WriteLine("Connecting to AV Manager at " & sserver & "...")

                connect_av()

                For Each item In awritableid

                    get_writable_data(item, "PRE")

                Next

                Dim wv As DictionaryEntry

                For Each wv In htwritables

                    Console.WriteLine(wv.Key & "|" & wv.Value)

                Next

                pop_disks(svm)

                For Each item In adisks

                    Console.WriteLine(item)

                Next

                Console.WriteLine("Checking for Attached Writable Volumes...")

                For Each item In adisks

                    If htwritables.ContainsKey(item) = True Then

                        Console.WriteLine("Found Writable Volume attached to Backup VM")
                        Console.WriteLine(item)
                        Console.WriteLine("Checking if the Writable Volume is attached to a User")

                        Select Case htwritables(item)

                            Case "Attached"

                                awvinuse.Add(item)
                                Console.WriteLine("Writable Volume is currently Attached!")
                                Console.WriteLine("We need to detach this Writable Volume from the Backup VM...")
                                remove_hdd(get_disk_id(svm, item), svm)
                                Console.WriteLine("Verifying the attached Writable Volume was removed from the Backup VM")
                                adisks.Clear()
                                pop_disks(svm)

                                If adisks.Contains(item) = False Then

                                    Console.WriteLine("The attached writable volume was successfully removed from the Backup VM")
                                    Console.WriteLine("It is safe to backup this VM")

                                    write_xml()

                                    End
                                End If


                            Case "Detached"

                                Console.WriteLine("Writable Volume is currently Detached - we will now disable it until the backup is done.")
                                Console.WriteLine("It is safe to Backup This VM")

                        End Select

                    End If

                Next


                End


            Case "POST"

                Console.WriteLine("Running post-backup cleanup processes...")
                Console.WriteLine("Connecting to AV Manager at " & sserver & "...")

                connect_av()

                For Each item In awritableid

                    get_writable_data(item, "POST")

                Next

                Console.WriteLine("Checking for XML File for this Backup VM")
                Dim sfilename As String = "AV_Backup_" & svm & ".xml"

                If File.Exists(sfilename) = True Then


                    Console.WriteLine("XML File Found at: " & "AV_Backup_" & svm & ".xml")
                    Console.WriteLine("Parsing XML file for Writable Volumes to Re-Connect to Backup VM")
                    read_xml(sfilename)

                    If awvtoattach.Count > 0 Then

                        Console.WriteLine("Reattaching Writable Volumes")

                        For Each item In awvtoattach

                            add_hdd(item, svm)

                        Next
                    End If

                Else

                    Console.WriteLine("XML File Not Found")


                End If

                End

            Case Else

                Console.WriteLine("Please enter PRE for pre-backup prep or POST for Post-backup cleanup as the last input parameter")
                Console.ReadLine()

        End Select


    End Sub

    Public Function AcceptAllCertifications(ByVal sender As Object, ByVal certification As System.Security.Cryptography.X509Certificates.X509Certificate, ByVal chain As System.Security.Cryptography.X509Certificates.X509Chain, ByVal sslPolicyErrors As System.Net.Security.SslPolicyErrors) As Boolean
        Return True
    End Function


    Sub read_xml(ByVal sfile As String)


        Dim xml As XElement = XElement.Load(sfile)

        Dim WV As IEnumerable(Of XElement) = xml.Descendants("WV")

        Dim sdisk As String = ""

        For Each Name As XElement In WV

            sdisk = (WV.Value)
            Console.WriteLine("Found Writable Volume to Re-Attach to Backup VM:")
            Console.WriteLine(sdisk)
            awvtoattach.Add(sdisk)

        Next

        Console.WriteLine("Finished with XML File")
        xml = Nothing

        File.Delete(sfile)

        If File.Exists(sfile) = False Then

            Console.WriteLine("XML File Removed")

        End If


    End Sub




    Sub remove_hdd(ByVal sdisk As String, ByVal vm As String)

        Console.WriteLine("Removing Attached Writable Volume from Backup VM")

        Dim instance As RunspaceConfiguration = RunspaceConfiguration.Create()
        Dim sname As String = "VMware.VimAutomation.Core"
        Dim warning As PSSnapInException
        Dim returnValue As PSSnapInInfo = instance.AddPSSnapIn(sname, warning)
        Dim runspace As Runspace = RunspaceFactory.CreateRunspace(instance)

        runspace.Open()

        Dim pipeline As Pipeline = runspace.CreatePipeline

        If InStr(susername, "\\") > 0 Then

            susername = Replace(susername, "\\", "\")

        End If

        pipeline.Commands.AddScript("Connect-VIServer -Server " & svc & " -user " & susername & " -password " & spw & "")
        pipeline.Commands.AddScript("Get-HardDisk -VM """ & vm & """ -Name """ & sdisk & """ | Remove-HardDisk -Confirm:$false")

        Dim thereturnValue As Collection(Of PSObject) = pipeline.Invoke

        runspace.Close()


    End Sub

    Function get_disk_id(ByVal svm As String, ByVal spath As String) As String

        Console.WriteLine("Getting Disk ID of the Writable Volume")

        Dim instance As RunspaceConfiguration = RunspaceConfiguration.Create()
        Dim sname As String = "VMware.VimAutomation.Core"
        Dim warning As PSSnapInException
        Dim returnValue As PSSnapInInfo = instance.AddPSSnapIn(sname, warning)
        Dim runspace As Runspace = RunspaceFactory.CreateRunspace(instance)

        runspace.Open()

        Dim pipeline As Pipeline = runspace.CreatePipeline


        If InStr(susername, "\\") > 0 Then

            susername = Replace(susername, "\\", "\")

        End If

        pipeline.Commands.AddScript("Connect-VIServer -Server " & svc & " -user " & susername & " -password " & spw & "")
        pipeline.Commands.AddScript("Get-HardDisk -VM """ & svm & """")

        Dim thereturnValue As Collection(Of PSObject) = pipeline.Invoke

        Dim sreturn As String = "The following VM's were removed: " & vbCrLf

        For Each result As PSObject In thereturnValue

            If result.Properties("Filename").Value.ToString = spath Then

                Return result.Properties("Name").Value.ToString

            End If

        Next

        runspace.Close()

        Return sreturn

    End Function

    Sub write_xml()

        Dim sfilename As String = "AV_Backup_" & svm & ".xml"

        Dim myElement As String = "<AV_Backup><Backup_VM>" & svm & "</Backup_VM>"

        For Each item In awvinuse

            myElement = myElement + "<WV>" & item & "</WV>"

        Next

        myElement = myElement + "</AV_Backup>"

        Dim xml As XElement = XElement.Parse(myElement)

        xml.Save(sfilename)


    End Sub


    Sub connect_av()


        If InStr(susername, "\") Then

            susername = Replace(susername, "\", "\\")

        End If


        Dim jsonstring As String = "{""username"" : """ & susername & """ , ""password"" : """ & spw & """}"

        Dim data = Encoding.UTF8.GetBytes(jsonstring)

        theuri = New Uri(sserver & "/cv_api/sessions")
        newuri = New Uri(sserver & "/cv_api/writables")

        SendRequest(theuri, data, "application/json", "POST")

        getdata(newuri, "application/json", "GET", "writables")


    End Sub


    Sub SendRequest(uri As Uri, jsonDataBytes As Byte(), contentType As String, method As String)

        Dim res As String = ""

        Try

            Dim req As HttpWebRequest = WebRequest.Create(uri)
            req.ContentType = contentType
            req.Method = method
            req.ContentLength = jsonDataBytes.Length
            req.CookieContainer = cookies
            req.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf AcceptAllCertifications)
            Dim stream = req.GetRequestStream()
            stream.Write(jsonDataBytes, 0, jsonDataBytes.Length)
            stream.Close()

            Dim response As HttpWebResponse = req.GetResponse
            Dim jsonresponse = req.GetResponse().GetResponseStream()

            For Each cookieValue As Cookie In response.Cookies

                ssession = cookieValue.ToString

            Next

            Console.WriteLine(ssession)

            Dim reader As New StreamReader(jsonresponse)
            res = reader.ReadToEnd()

            reader.Close()
            response.Close()

        Catch ex As Exception

            Console.WriteLine(ex.Message)

        End Try


    End Sub

    Sub getdata(uri As Uri, contentType As String, method As String, ByVal thequery As String)

        Dim res As String = ""

        Console.WriteLine("Getting Writable Volumes...")

        Try

            Dim req As HttpWebRequest = WebRequest.Create(uri)

            req.ContentType = contentType
            req.Method = method
            req.CookieContainer = cookies
            req.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf AcceptAllCertifications)
            Dim jsonresponse = req.GetResponse().GetResponseStream()
            Dim reader As New StreamReader(jsonresponse)
            res = reader.ReadToEnd()

            reader.Close()

            Dim jss As New JavaScriptSerializer()

            Select Case thequery

                Case "storages"
                    Dim ajson As Array = Split(res, ":[")

                    Dim newjson As String = "[" + ajson(1)

                    newjson = newjson.Remove(newjson.Length - 1)

                    Dim lstTextAreas As List(Of storage) = jss.Deserialize(newjson, GetType(List(Of storage)))

                    For i = 0 To lstTextAreas.Count - 1

                        MsgBox(lstTextAreas(i).name)

                    Next i

                Case "appstacks"

                    Dim lstTextAreas As List(Of appstacks) = jss.Deserialize(res, GetType(List(Of appstacks)))

                    For i = 0 To lstTextAreas.Count - 1


                    Next i

                Case "writables"

                    Dim ajson As Array = Split(res, ":[")

                    Dim newjson As String = "[" + ajson(1)

                    newjson = newjson.Remove(newjson.Length - 2)

                    Dim anewjson As Array = Split(newjson, "]")

                    newjson = anewjson(0) + "]"

                    Dim lstTextAreas As List(Of writables) = jss.Deserialize(newjson, GetType(List(Of writables)))

                    Console.WriteLine("Found " & lstTextAreas.Count & " Writable Volumes...")

                    For i = 0 To lstTextAreas.Count - 1

                        Dim iasid As Integer = lstTextAreas(i).id

                        awritableid.Add(iasid)

                    Next i

                Case Else

            End Select


        Catch ex As Exception

            If InStr(ex.Message, "403") Then

                MsgBox("Session expired. Please login again.")

            Else

                MsgBox(ex.Message)

            End If

            'MsgBox(ex.Message)

        End Try

    End Sub

    Sub get_writable_data(ByVal sid As String, ByVal saction As String)

        Dim Uri As Uri = New Uri(sserver & "/cv_api/writables/" & sid)

        Dim res As String = ""

        Try
            Dim req As HttpWebRequest = WebRequest.Create(Uri)

            req.ContentType = "application/json"
            req.Method = "GET"
            req.CookieContainer = cookies
            req.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf AcceptAllCertifications)
            Dim jsonresponse = req.GetResponse().GetResponseStream()
            Dim reader As New StreamReader(jsonresponse)
            res = reader.ReadToEnd()

            reader.Close()

            Dim jss As New JavaScriptSerializer()

            Dim ajson As Array = Split(res, ":{")

            Dim newjson As String = "[{" + ajson(1)

            newjson = newjson.Remove(newjson.Length - 1)

            newjson = newjson + "]"

            Dim lstTextAreas As List(Of WritableData) = jss.Deserialize(newjson, GetType(List(Of WritableData)))

            For i = 0 To lstTextAreas.Count - 1

                htwritables.Add(lstTextAreas(i).file_location, lstTextAreas(i).attached)

                Select Case saction

                    Case "PRE"

                        If lstTextAreas(i).attached = "Detached" Then

                            Console.WriteLine("Writable Volume " & sid & " is detached.  Disabling it until the backup is finished.")
                            disable_writable(sid)

                        End If

                    Case "POST"

                        If lstTextAreas(i).attached = "Detached" Then

                            Console.WriteLine("Writable Volume " & sid & " is detached.  Disabling it until the backup is finished.")
                            enable_writable(sid)

                        End If

                End Select


            Next i

        Catch ex As Exception

            Console.WriteLine(ex.Message)

        End Try


    End Sub

    Function add_hdd(ByVal sdisk As String, ByVal svm As String) As String

        Dim instance As RunspaceConfiguration = RunspaceConfiguration.Create()
        Dim sname As String = "VMware.VimAutomation.Core"
        Dim warning As PSSnapInException
        Dim returnValue As PSSnapInInfo = instance.AddPSSnapIn(sname, warning)
        Dim runspace As Runspace = RunspaceFactory.CreateRunspace(instance)

        Dim stheuser As String = susername

        If InStr(stheuser, "\\") > 0 Then

            stheuser = Replace(stheuser, "\\", "\")

        End If

        Try
            runspace.Open()

            Dim pipeline As Pipeline = runspace.CreatePipeline

            pipeline.Commands.AddScript("Connect-VIServer -Server " & svc & " -user " & stheuser & " -password " & spw & "")
            pipeline.Commands.AddScript("New-HardDisk -VM """ & svm & """ -DiskPath """ & sdisk & """")

            Dim thereturnValue As Collection(Of PSObject) = pipeline.Invoke

            Dim sfilename As String

            For Each result As PSObject In thereturnValue

                sfilename = result.Properties("Filename").Value.ToString

                Return ("Added Disk: " & sfilename & " to the backup VM " & svm)

            Next

            runspace.Close()

        Catch ex As Exception

            MsgBox(ex.Message)

        End Try

    End Function

    Sub pop_disks(ByVal svmname As String)

        Console.WriteLine("Getting Disks attached to Backup VM " & svm)

        Dim instance As RunspaceConfiguration = RunspaceConfiguration.Create()
        Dim sname As String = "VMware.VimAutomation.Core"
        Dim warning As PSSnapInException
        Dim returnValue As PSSnapInInfo = instance.AddPSSnapIn(sname, warning)
        Dim runspace As Runspace = RunspaceFactory.CreateRunspace(instance)

        runspace.Open()

        Dim pipeline As Pipeline = runspace.CreatePipeline

        If InStr(susername, "\\") > 0 Then

            susername = Replace(susername, "\\", "\")

        End If

        pipeline.Commands.AddScript("Connect-VIServer -Server " & svc & " -user " & susername & " -password " & spw & "")
        pipeline.Commands.AddScript("Get-HardDisk -VM " & svmname & "")

        Dim thereturnValue As Collection(Of PSObject) = pipeline.Invoke

        Dim thedisks As New ArrayList
        Dim asname As New ArrayList
        Dim sdisk As String = ""

        For Each id In thereturnValue

            sdisk = id.Properties("Filename").Value.ToString
            adisks.Add(sdisk)

        Next

        runspace.Close()

    End Sub

    Sub disable_writable(ByVal sid As String)

        Dim Uri As Uri = New Uri(sserver & "/cv_api/writables/disable?volumes%5B%5D=" & sid)

        Dim res As String = ""

        Try
            Dim req As HttpWebRequest = WebRequest.Create(Uri)

            req.ContentType = "application/json"
            req.Method = "POST"
            req.CookieContainer = cookies
            req.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf AcceptAllCertifications)
            Dim jsonresponse = req.GetResponse().GetResponseStream()
            Dim reader As New StreamReader(jsonresponse)
            res = reader.ReadToEnd()

            reader.Close()

            Console.WriteLine(res)

        Catch ex As Exception

            Console.WriteLine(ex.Message)

        End Try


    End Sub

    Sub enable_writable(ByVal sid As String)

        Dim Uri As Uri = New Uri(sserver & "/cv_api/writables/enable?volumes%5B%5D=" & sid)

        Dim res As String = ""

        Try
            Dim req As HttpWebRequest = WebRequest.Create(Uri)

            req.ContentType = "application/json"
            req.Method = "POST"
            req.CookieContainer = cookies
            req.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf AcceptAllCertifications)
            Dim jsonresponse = req.GetResponse().GetResponseStream()
            Dim reader As New StreamReader(jsonresponse)
            res = reader.ReadToEnd()

            reader.Close()

            Console.WriteLine(res)

        Catch ex As Exception

            Console.WriteLine(ex.Message)

        End Try


    End Sub

End Module
