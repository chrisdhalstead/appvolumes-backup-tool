Imports VMware.Vim
Imports System.Collections.Specialized
Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Web.Script.Serialization
Imports System.Web
Imports Backup_AppStacks.AppVolumes
Imports System.Collections.ObjectModel
Imports System.Management.Automation
Imports System.Management.Automation.Runspaces
Imports Microsoft.Win32
Imports System.Net.Security

Public Class Form1
    Public theuri As Uri
    Public newuri As Uri
    Public t As System.Threading.Thread
    Public ssession As String
    Public avversion As String
    Public sresult As String
    Public cookies As New CookieContainer
    Public nitem As New ListViewItem
    Public c As New VimClientImpl
    Public cclient As VimClient
    Public conn As ServiceContent
    Public us As New UserSession
    Public tv As New TreeNode
    Public saspath As String = ""
    Public vcusername As String = ""
    Public vcpw As String = ""
    Public ahosts As New ArrayList
    Public abadhosts As New ArrayList
    Public vc As String = ""
    Public vcps As String = ""
    Public currentas As String = ""
    Public aasids As New ArrayList
    Public awritableids As New ArrayList
    Public htas As New Hashtable
    Public htwritables As New Hashtable
    Public savmanager As String = ""
    Public shost As String = ""
    Public ssessionid As String = ""
    Public spssessionid As String = ""
    Public htvmhost As New Hashtable
    Public sessionmanager As SessionManager
    Public instance As RunspaceConfiguration = RunspaceConfiguration.Create()
    Public sname As String = "VMware.VimAutomation.Core"
    Public warning As PSSnapInException
    Public returnValue As PSSnapInInfo = instance.AddPSSnapIn(sname, warning)
    Public runspace As Runspace = RunspaceFactory.CreateRunspace(instance)

    Private Sub cmdpopAS_Click(sender As Object, e As EventArgs) Handles cmdpopAS.Click

        If ssession = "" Then

            dlgav.ShowDialog()

            Exit Sub

        Else

            get_initial_av_data()

        End If

    End Sub

    Sub get_initial_av_data()

        aasids.Clear()
        awritableids.Clear()
        lstAS.Items.Clear()
        htas.Clear()
        htwritables.Clear()

        newuri = New Uri(txtAVManager.Text & "/cv_api/writables")
        getdata(newuri, "application/json", "GET", "writables")

        newuri = New Uri(txtAVManager.Text & "/cv_api/appstacks")
        getdata(newuri, "application/json", "GET", "appstacks")

        sessionstatus.Text = "App Volumes Version: " & get_av_version()

        For Each item In aasids

            Dim sname As String = get_as_name(item)

            For Each path In pop_as_details(item)

                If htas.ContainsKey(path) = False Then

                    htas.Add(path, sname)

                End If

            Next

        Next

        For Each item In awritableids

            Dim swvname As String = get_writable_name(item)

            Dim awv As Array = Split(swvname, "|")

            htwritables.Add(awv(1), awv(0))

        Next



    End Sub

    Public Function AcceptAllCertifications(ByVal sender As Object, ByVal certification As System.Security.Cryptography.X509Certificates.X509Certificate, ByVal chain As System.Security.Cryptography.X509Certificates.X509Chain, ByVal sslPolicyErrors As System.Net.Security.SslPolicyErrors) As Boolean
        Return True
    End Function


    Function get_as_ds(ByVal sid As String) As Integer

        Dim Uri As Uri = New Uri(txtAVManager.Text & "/cv_api/appstacks/" & sid)

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

            Dim lstTextAreas As List(Of AppStackData) = jss.Deserialize(newjson, GetType(List(Of AppStackData)))

            If lstTextAreas.Count = 0 Then

                Return "False"

            End If

            For i = 0 To lstTextAreas.Count - 1

                Return lstTextAreas(i).location_count

            Next i

        Catch ex As Exception

            If InStr(ex.Message, "403") Then

                MsgBox("Session expired. Please login again.")

            Else

                MsgBox(ex.Message)

            End If

            'MsgBox(ex.Message)

        End Try


    End Function

    Function get_as_locations(ByVal sid As String) As Integer


        Dim Uri As Uri = New Uri(txtAVManager.Text & "/cv_api/appstacks/" & sid & "/files")

        Dim res As String = ""

        Try

            Dim req As HttpWebRequest = WebRequest.Create(Uri)
            DlgDS.htdsinfo.Clear()
            req.ContentType = "application/json"
            req.Method = "GET"
            req.CookieContainer = cookies
            req.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf AcceptAllCertifications)
            Dim jsonresponse = req.GetResponse().GetResponseStream()
            Dim reader As New StreamReader(jsonresponse)
            res = reader.ReadToEnd()

            reader.Close()

            Dim jss As New JavaScriptSerializer()

            Dim lstTextAreas As List(Of file_locations) = jss.Deserialize(res, GetType(List(Of file_locations)))

            If lstTextAreas.Count = 0 Then

                Return "False"

            End If

            DlgDS.htdsinfo.Clear()

            For i = 0 To lstTextAreas.Count - 1

                Dim slocation As String = lstTextAreas(i).storage_location

                If DlgDS.htdsinfo.ContainsKey(slocation) = False Then

                    DlgDS.htdsinfo.Add(slocation, lstTextAreas(i).missing & "|" & lstTextAreas(i).reachable & "|" & lstTextAreas(i).machine_manager_host)

                    DlgDS.cmbDataStore.Items.Add(slocation)

                End If

            Next i

        Catch ex As Exception

            If InStr(ex.Message, "403") Then

                MsgBox("Session expired. Please login again.")

            Else

                MsgBox(ex.Message)

            End If


        End Try


    End Function

    Function get_file_locations(ByVal sid As String) As ArrayList

        Dim adisks As New ArrayList

        Dim Uri As Uri = New Uri(txtAVManager.Text & "/cv_api/appstacks/" & sid & "/files")

        Dim res As String = ""

        Try

            Dim req As HttpWebRequest = WebRequest.Create(Uri)
            DlgDS.htdsinfo.Clear()
            req.ContentType = "application/json"
            req.Method = "GET"
            req.CookieContainer = cookies
            req.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf AcceptAllCertifications)
            Dim jsonresponse = req.GetResponse().GetResponseStream()
            Dim reader As New StreamReader(jsonresponse)
            res = reader.ReadToEnd()

            reader.Close()

            Dim jss As New JavaScriptSerializer()

            Dim lstTextAreas As List(Of file_locations) = jss.Deserialize(res, GetType(List(Of file_locations)))

            If lstTextAreas.Count = 0 Then

                Return adisks

            End If


            For i = 0 To lstTextAreas.Count - 1

                Dim slocation As String = lstTextAreas(i).storage_location

                adisks.Add(slocation)


            Next i

        Catch ex As Exception

            If InStr(ex.Message, "403") Then

                MsgBox("Session expired. Please login again.")

            Else

                MsgBox(ex.Message)

            End If

            'MsgBox(ex.Message)

        End Try


        Return adisks

    End Function

    Function pop_as_details(ByVal sid As String) As ArrayList

        Dim adisks As New ArrayList

        If ssession = "" Then

            MsgBox("Your session to the App Volumes API Expired.  Please login again", MsgBoxStyle.Exclamation, "Session Expired")
            dlgav.ShowDialog()

        End If

        Dim Uri As Uri = New Uri(txtAVManager.Text & "/cv_api/appstacks/" & sid & "/files")

        Dim res As String = ""

        Try

            Dim req As HttpWebRequest = WebRequest.Create(Uri)
            DlgDS.htdsinfo.Clear()
            req.ContentType = "application/json"
            req.Method = "GET"
            req.CookieContainer = cookies
            req.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf AcceptAllCertifications)
            Dim jsonresponse = req.GetResponse().GetResponseStream()
            Dim reader As New StreamReader(jsonresponse)
            res = reader.ReadToEnd()

            reader.Close()

            Dim jss As New JavaScriptSerializer()

            Dim lstTextAreas As List(Of file_locations) = jss.Deserialize(res, GetType(List(Of file_locations)))

            If lstTextAreas.Count = 0 Then

                Return adisks

            End If

            For i = 0 To lstTextAreas.Count - 1


                Dim breachable As Boolean = lstTextAreas(i).reachable
                Dim bmissing As Boolean = lstTextAreas(i).missing

                If breachable = True And bmissing = False Then

                    Dim slocation As String = lstTextAreas(i).storage_location

                    slocation = "[" + slocation + "]"

                    slocation = slocation + " " & lstTextAreas(i).path + "/" & lstTextAreas(i).name

                    adisks.Add(slocation)

                End If


            Next i

        Catch ex As Exception

            If InStr(ex.Message, "403") Then

                MsgBox("Session expired. Please login again.")

            Else

                MsgBox(ex.Message)

            End If

            'MsgBox(ex.Message)

        End Try


        Return adisks

    End Function


    Function get_as_data(ByVal sid As String) As String

        Dim Uri As Uri = New Uri(txtAVManager.Text & "/cv_api/appstacks/" & sid)

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

            Dim lstTextAreas As List(Of AppStackData) = jss.Deserialize(newjson, GetType(List(Of AppStackData)))

            If lstTextAreas.Count = 0 Then

                Return "False"

            End If

            For i = 0 To lstTextAreas.Count - 1

                Return lstTextAreas(i).file_location

            Next i

        Catch ex As Exception

            If InStr(ex.Message, "403") Then

                MsgBox("Session expired. Please login again.")

            ElseIf ex.Message = "Cannot convert null to a value type." Then


            Else

                MsgBox(ex.Message, MsgBoxStyle.Information, "Error")

            End If

            'MsgBox(ex.Message)

        End Try


    End Function

    Function get_writable_data(ByVal sid As String) As String

        Dim Uri As Uri = New Uri(txtAVManager.Text & "/cv_api/writables/" & sid)

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

                'Return (lstTextAreas(0).name.ToString())

                Return lstTextAreas(i).file_location

            Next i

        Catch ex As Exception

            If InStr(ex.Message, "403") Then

                MsgBox("Session expired. Please login again.")

            Else

                MsgBox(ex.Message)

            End If

            'MsgBox(ex.Message)

        End Try


    End Function



    Sub get_as_apps(ByVal sid As String)

        dlgApps.lstapps.Items.Clear()

        Dim Uri As Uri = New Uri(txtAVManager.Text & "/cv_api/appstacks/" & sid & "/applications")

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

            Dim ajson As Array = Split(res, ":[")

            Dim newjson As String = "[" + ajson(1)

            newjson = newjson.Remove(newjson.Length - 1)

            Dim lstTextAreas As List(Of applications) = jss.Deserialize(newjson, GetType(List(Of applications)))

            For i = 0 To lstTextAreas.Count - 1

                'Return (lstTextAreas(0).name.ToString())

                Dim sname As String = ""
                Dim spublisher As String = ""
                Dim sversion As String = ""

                sname = lstTextAreas(i).name
                spublisher = lstTextAreas(i).publisher
                sversion = lstTextAreas(i).version

                If sname = "" Then

                    sname = "N/A"

                End If

                If spublisher = "" Then

                    spublisher = "N/A"

                End If

                If sversion = "" Then

                    sversion = "N/A"

                End If

                nitem = dlgApps.lstapps.Items.Add(sname)
                nitem.SubItems.Add(spublisher)
                nitem.SubItems.Add(sversion)

            Next i

        Catch ex As Exception

            If InStr(ex.Message, "403") Then

                MsgBox("Session expired. Please login again.")

            Else

                MsgBox(ex.Message)

            End If

            'MsgBox(ex.Message)

        End Try


    End Sub


    Function get_as_name(ByVal sid As Integer) As String

        If ssession = "" Then

            MsgBox("Your session to the App Volumes API Expired.  Please login again", MsgBoxStyle.Exclamation, "Session Expired")
            dlgav.ShowDialog()

        End If

        Dim Uri As Uri = New Uri(txtAVManager.Text & "/cv_api/appstacks/" & sid)

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

            Dim lstTextAreas As List(Of AppStackData) = jss.Deserialize(newjson, GetType(List(Of AppStackData)))

            For i = 0 To lstTextAreas.Count - 1

                Return lstTextAreas(0).name.ToString()


            Next i

        Catch ex As Exception

            If InStr(ex.Message, "403") Then

                MsgBox("Session expired. Please login again.")

            Else

                MsgBox(ex.Message)

            End If

            'MsgBox(ex.Message)

        End Try


    End Function


    Function get_av_version() As String

        If ssession = "" Then

            MsgBox("Your session to the App Volumes API Expired.  Please login again", MsgBoxStyle.Exclamation, "Session Expired")
            dlgav.ShowDialog()

        End If

        Dim Uri As Uri = New Uri(txtAVManager.Text & "/cv_api/version")

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

            Dim newjson As String = res

            newjson = "[" + newjson + "]"

            Dim lstTextAreas As List(Of Version) = jss.Deserialize(newjson, GetType(List(Of Version)))

            For i = 0 To lstTextAreas.Count - 1

                Return lstTextAreas(0).version.ToString()

            Next i

        Catch ex As Exception

            If InStr(ex.Message, "403") Then

                MsgBox("Session expired. Please login again.")

            Else

                MsgBox(ex.Message)

            End If

            'MsgBox(ex.Message)

        End Try


    End Function

    Function get_writable_name(ByVal sid As Integer) As String

        If ssession = "" Then

            MsgBox("Your session to the App Volumes API Expired.  Please login again", MsgBoxStyle.Exclamation, "Session Expired")
            dlgav.ShowDialog()

        End If

        Dim Uri As Uri = New Uri(txtAVManager.Text & "/cv_api/writables/" & sid)

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

            If lstTextAreas.Count = 0 Then

                Return "False"

            End If

            For i = 0 To lstTextAreas.Count - 1

                Dim sstring = "Writable Volume: " & lstTextAreas(0).name.ToString() & "|" & lstTextAreas(i).file_location

                Return sstring

            Next i

        Catch ex As Exception


            If InStr(ex.Message, "404") Then

                Return "False"

            End If

            If InStr(ex.Message, "403") Then

                MsgBox("Session expired. Please login again.")

                Return False

            Else

                MsgBox(ex.Message)
                Return False

            End If

            'MsgBox(ex.Message)

        End Try

        Return False

    End Function


    Sub getdata(uri As Uri, contentType As String, method As String, ByVal thequery As String)

        Dim res As String = ""

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

                        nitem = lstAS.Items.Add(lstTextAreas(i).name, 1)

                        Dim sresult As Integer = 0

                        sresult = get_as_ds(lstTextAreas(i).id)

                        If sresult > 1 Then

                            nitem.SubItems.Add("Multiple")

                        Else

                            nitem.SubItems.Add(lstTextAreas(i).datastore_name)

                        End If

                        nitem.SubItems.Add(lstTextAreas(i).size_mb)
                        nitem.SubItems.Add(lstTextAreas(i).attachments_total)

                        Dim iasid As Integer = lstTextAreas(i).id

                        aasids.Add(iasid)
                        nitem.SubItems.Add(iasid)

                        nitem.SubItems.Add(lstTextAreas(i).status)

                    Next i

                Case "writables"

                    Dim ajson As Array = Split(res, ":[")

                    Dim newjson As String = "[" + ajson(1)

                    newjson = newjson.Remove(newjson.Length - 2)

                    Dim anewjson As Array = Split(newjson, "]")

                    newjson = anewjson(0) + "]"

                    Dim lstTextAreas As List(Of writables) = jss.Deserialize(newjson, GetType(List(Of writables)))

                    For i = 0 To lstTextAreas.Count - 1

                        nitem = lstAS.Items.Add(lstTextAreas(i).name, 2)
                        nitem.SubItems.Add(lstTextAreas(i).datastore_name)

                        Dim sspace As String = lstTextAreas(i).total_mb - lstTextAreas(i).free_mb

                        nitem.SubItems.Add(sspace)

                        Dim sattached As String = lstTextAreas(i).attached

                        Select Case sattached

                            Case "Attached"

                                nitem.SubItems.Add(sattached)

                            Case "Detached"

                                nitem.SubItems.Add(sattached)

                        End Select

                        Dim iasid As Integer = lstTextAreas(i).id

                        awritableids.Add(iasid)
                        nitem.SubItems.Add(iasid)



                        nitem.SubItems.Add(lstTextAreas(i).status)

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


    Private Sub cmdConnecttoVC_Click(sender As Object, e As EventArgs) Handles cmdConnecttoVC.Click

        If ssessionid = "" Then

            dlgvc.ShowDialog()

            Exit Sub

        End If

        vc = txtvc.Text

        pb1.Visible = True
        pb1.BringToFront()

        t = New System.Threading.Thread(AddressOf get_data)

        t.Start()


    End Sub

    Function add_hdd(ByVal sdisk As String, ByVal svm As String) As String

        Dim pipeline As Pipeline = runspace.CreatePipeline

        pipeline.Commands.AddScript("New-HardDisk -VM """ & svm & """ -DiskPath """ & sdisk & """")

        Dim thereturnValue As Collection(Of PSObject) = pipeline.Invoke

        Dim sfilename As String

        For Each result As PSObject In thereturnValue

            sfilename = result.Properties("Filename").Value.ToString

            Return (htas(sfilename))

        Next

    End Function

    Sub create_vm(ByVal sds As String, ByVal shost As String)

        pb1.Visible = True
        pb1.BringToFront()

        Dim pipeline As Pipeline = runspace.CreatePipeline

        Dim sdate As String = Now.ToShortDateString
        sdate = Replace(sdate, "/", "_")

        Dim sname As String = "_avbackup_" & sdate
        Dim snewname As String = sname

        Dim i As Integer = 1

        If htvmhost.ContainsKey(snewname) = True Then

            Do Until htvmhost.ContainsKey(snewname) = False

                snewname = sname & "_" & i

                i += 1

            Loop

        Else

            snewname = sname

        End If

        sessionstatus.Text = "Creating Backup VM: " & snewname

        pipeline.Commands.AddScript("new-vm -name " & snewname & " -datastore """ & sds & """ -diskmb 1 -host """ & shost & """")

        Dim thereturnValue As Collection(Of PSObject) = pipeline.Invoke

        Dim sreturn As String = "The following Backup VM was added: " & vbCrLf

        For Each result As PSObject In thereturnValue

            sreturn = sreturn & result.Properties("name").Value.ToString & vbCrLf

        Next

        MsgBox(sreturn, MsgBoxStyle.Information, "Backup VM Created")

        get_data()

    End Sub

    Public Shared Function GetChildren(objTree As TreeView) As List(Of TreeNode)
        Dim nodes As List(Of TreeNode) = New List(Of TreeNode)
        For Each parentNode As TreeNode In objTree.Nodes
            nodes.Add(parentNode)
            GetAllChildren(parentNode, nodes)
        Next

        Return nodes
    End Function

    Public Shared Sub GetAllChildren(parentNode As TreeNode, nodes As List(Of TreeNode))
        For Each childNode As TreeNode In parentNode.Nodes
            nodes.Add(childNode)
            GetAllChildren(childNode, nodes)
        Next
    End Sub


    Private Sub cmdAttachAS_Click(sender As Object, e As EventArgs) Handles cmdAttachAS.Click


        Dim squestion As String = "Are you sure you want to attach the following AppStacks?" & vbCrLf & vbCrLf

        Dim item As ListViewItem
        Dim inode As TreeNode


        For Each item In lstAS.CheckedItems

            squestion = squestion & "*-" & item.Text & vbCrLf

        Next

        If MsgBox(squestion, MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Attach AppStacks?") = vbYes Then

            Dim selecteddata As List(Of TreeNode) = GetChildren(tv_vcenter)

            Dim svm As String = ""

            For Each inode In selecteddata

                If inode.Checked = True And inode.Level = "2" Then

                    Dim avm As Array = Split(inode.Text, "VM: ")

                    If inode.Parent.Name <> "OK" Then


                        MsgBox("Cannot use this Backup VM.  The host it is on has the following issue: " & inode.Parent.Name, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Error with Host")
                        Exit Sub


                    End If

                    svm = avm(1)

                End If

            Next

            If svm = "" Then

                MsgBox("Please select a Backup VM", MsgBoxStyle.Information, "Select a Backup VM")
                Exit Sub

            End If

            For Each item In lstAS.CheckedItems

                Dim svols As String = item.SubItems(1).Text
                Dim sid As String = item.SubItems(4).Text

                If svols = "Multiple" Then

                    DlgDS.cmbDataStore.Items.Clear()
                    DlgDS.cmbDataStore.Text = ""
                    DlgDS.chkisreachable.CheckState = CheckState.Indeterminate
                    DlgDS.chkexists.CheckState = CheckState.Indeterminate
                    DlgDS.lblhostname.Text = ""
                    DlgDS.lblas.Text = ""

                    DlgDS.lblas.Text = item.Text

                    DlgDS.Text = "Select Datastore location for " & item.Text

                    get_as_locations(sid)

                    DlgDS.ShowDialog(Me)

                End If

            Next

            t = New System.Threading.Thread(AddressOf add_drives)

            t.Start()

        End If


        'MsgBox(sreturn, MsgBoxStyle.Information, "Added AppStacks")


    End Sub

    Sub add_drives()

        Dim sstring As String = ""

        Dim sid As String = ""

        Dim iindex As Integer

        Dim selecteddata As List(Of TreeNode) = GetChildren(tv_vcenter)

        Dim item As TreeNode

        Dim svm As String = ""

        For Each item In selecteddata

            If item.Checked = True And item.Level = "2" Then

                Dim avm As Array = Split(item.Text, "VM: ")

                svm = avm(1)

            End If

        Next

        If svm = "" Then

            MsgBox("Please select a Backup VM", MsgBoxStyle.Information, "Select a Backup VM")
            Exit Sub

        End If

        pb1.Visible = True
        pb1.BringToFront()

        Dim sreturn As String = "Added the Following AppStacks: " & vbCrLf

        Dim thing As ListViewItem

        Dim sasname As String = ""

        For Each thing In lstAS.CheckedItems

            thing.Checked = False

            iindex = thing.Index

            currentas = ""

            sid = lstAS.Items(iindex).SubItems(4).Text

            sessionstatus.Text = "Adding Drive: " & thing.Text

            Dim slocation As String = ""


            If thing.ImageIndex = "2" Then

                slocation = get_writable_data(sid)

            Else


                slocation = get_as_data(sid)

            End If

            Dim alocation As Array = Split(slocation, "]")

            slocation = "[" & thing.SubItems(1).Text & "]" & alocation(1)

            add_hdd(slocation, svm)

            sessionstatus.Text = "Added Drive: " & thing.Text

        Next

        sessionstatus.Text = ""

        'MsgBox("The AppStacks were added to the Backup VM", MsgBoxStyle.Information, "Added AppStacks to " & svm)
        get_data()

        pb1.Visible = False

    End Sub


    Function pop_disks(ByVal svmname As String) As ArrayList

        Dim pipeline As Pipeline = runspace.CreatePipeline

        pipeline.Commands.AddScript("Get-HardDisk -VM """ & svmname & """")

        Dim thereturnValue As Collection(Of PSObject) = pipeline.Invoke

        Dim thedisks As New ArrayList
        Dim asname As New ArrayList
        Dim sdisk As String = ""

        For Each id In thereturnValue

            sdisk = id.Properties("Filename").Value.ToString

            If InStr(sdisk, svmname) > 0 Then

            Else

                thedisks.Add(sdisk)

            End If

        Next

        If thedisks.Count = 0 Then

            asname.Add("** No AppStacks Attached **")

            Return asname

        End If

        For Each item In thedisks

            asname.Add(item)

        Next

        Return asname


    End Function

    Function connect_ps() As String

        Dim pipeline As Pipeline = runspace.CreatePipeline
        pipeline.Commands.AddScript("$srv = Connect-VIServer -Server " & vcps & " -user " & vcusername & " -password " & vcpw & "")
        pipeline.Commands.AddScript("write-output $srv.SessionID")

        Dim thereturnValue As Collection(Of PSObject) = pipeline.Invoke

        Return thereturnValue(0).ToString

    End Function

    Sub disconnect_ps()

        Dim pipeline As Pipeline = runspace.CreatePipeline
        ' pipeline.Commands.AddScript("write-output $srv.SessionId")
        pipeline.Commands.AddScript("Disconnect-VIServer -Server " & vcps & " -Confirm:$false -Force -ErrorAction SilentlyContinue")

        Dim thereturnValue As Collection(Of PSObject) = pipeline.Invoke

        runspace.Close()

    End Sub

    Function get_disk_id(ByVal svm As String, ByVal spath As String) As String


        Dim pipeline As Pipeline = runspace.CreatePipeline

        pipeline.Commands.AddScript("Get-HardDisk -VM """ & svm & """")

        Dim thereturnValue As Collection(Of PSObject) = pipeline.Invoke

        Dim sreturn As String = "The following VM's were removed: " & vbCrLf

        For Each result As PSObject In thereturnValue

            If result.Properties("Filename").Value.ToString = spath Then

                Return result.Properties("Name").Value.ToString

            End If

        Next

        Return sreturn

    End Function

    Function remove_hdd(ByVal sdisk As String, ByVal vm As String) As String

        Dim pipeline As Pipeline = runspace.CreatePipeline

        pipeline.Commands.AddScript("Get-HardDisk -VM """ & vm & """ -Name """ & sdisk & """ | Remove-HardDisk -Confirm:$false")

        Dim thereturnValue As Collection(Of PSObject) = pipeline.Invoke

        Dim sreturn As String = "The following VM's were removed: " & vbCrLf

        For Each result As PSObject In thereturnValue

            sreturn = sreturn & result.Properties("Filename").Value.ToString & vbCrLf

        Next

        Return sreturn

    End Function

    Function remove_all_hdd(ByVal vm As String) As String

        Dim pipeline As Pipeline = runspace.CreatePipeline

        pipeline.Commands.AddScript("Get-HardDisk -VM """ & vm & """ | Remove-HardDisk -Confirm:$false")

        Dim thereturnValue As Collection(Of PSObject) = pipeline.Invoke

        Dim sreturn As String = "The following VM's were removed: " & vbCrLf

        For Each result As PSObject In thereturnValue

            sreturn = sreturn & result.Properties("Filename").Value.ToString & vbCrLf

        Next

        Return sreturn

    End Function



    Function remove_vm(ByVal svm As String) As String

        Dim pipeline As Pipeline = runspace.CreatePipeline

        pipeline.Commands.AddScript("Remove-VM -VM """ & svm & """ -DeleteFromDisk -Confirm:$false -RunAsync")

        Dim sreturn As String

        Try

            Dim thereturnValue As Collection(Of PSObject) = pipeline.Invoke

            sreturn = "The following VM's were removed: " & vbCrLf

            For Each result As PSObject In thereturnValue


            Next


        Catch ex As Exception


            Return ex.Message


        End Try




        Return sreturn



    End Function


    Private Sub cmdCreateVM_Click(sender As Object, e As EventArgs) Handles cmdCreateVM.Click


        If MsgBox("Are you sure you want to create a Backup VM?", MsgBoxStyle.Question + vbYesNo, "Create a Backup VM??") = vbYes Then

            dlgCreateVM.cmbds.Text = ""
            dlgCreateVM.cmbHosts.Text = ""
            dlgCreateVM.cmbHosts.Items.Clear()
            dlgCreateVM.cmbds.Items.Clear()

            For Each item In ahosts

                dlgCreateVM.cmbHosts.Items.Add(item)

            Next

            dlgCreateVM.ShowDialog()

        End If

    End Sub

    Sub remove_drives()

        pb1.Visible = True
        pb1.BringToFront()

        Dim selecteddata As List(Of TreeNode) = GetChildren(tv_vcenter)

        Dim item As TreeNode

        For Each item In selecteddata

            If item.Checked = True And item.Level = "3" Then

                item.Checked = False

                Dim avm As Array = Split(item.Parent.Text, "VM: ")

                Dim svm As String = avm(1)

                sessionstatus.Text = "Detaching " & item.Text & " from " & svm

                remove_hdd(get_disk_id(svm, item.Name), avm(1))

            End If


        Next

        sessionstatus.Text = ""

        get_data()

    End Sub

    Private Sub cmdDeleteVM_Click(sender As Object, e As EventArgs) Handles cmdDeleteVM.Click


        Dim selecteddata As List(Of TreeNode) = GetChildren(tv_vcenter)

        Dim item As TreeNode

        For Each item In selecteddata

            If item.Checked = True And item.Level = "2" Then

                Dim avm As Array = Split(item.Text, "VM: ")

                Dim svm As String = avm(1)

                If MsgBox("Are you sure you want to remove the Backup VM " & svm & "?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Delete VM?") = MsgBoxResult.Yes Then


                    remove_all_hdd(svm)
                    remove_vm(svm)


                End If


            End If

        Next



        get_data()

    End Sub

    Function Reg_Read(ByVal spath As String, ByVal sname As String) As String

        Dim regKey As RegistryKey
        Dim svalue As String

        Try

            regKey = Registry.CurrentUser.OpenSubKey(spath, True)

            svalue = regKey.GetValue(sname)
            If svalue = Nothing Then

                Return "NO_VALUE"

            End If

            Return svalue
            regKey.Close()

        Catch ex As Exception

            MessageBox.Show("Error: " & ex.Message)
            Return "ERROR"

        End Try

    End Function


    Function reg_key_exists(ByVal spath As String) As Boolean

        Dim regExists As RegistryKey
        regExists = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(spath, False)
        If regExists Is Nothing Then

            Return False

        Else

            Return True

        End If
    End Function

    Sub Reg_Write(ByVal spath As String, ByVal sname As String, ByVal svalue As String)

        Dim regKey As RegistryKey

        Try

            regKey = Registry.CurrentUser.OpenSubKey(spath, True)
            regKey.SetValue(sname, svalue)
            regKey.Close()

        Catch ex As Exception

            MessageBox.Show("Error: " & ex.Message)


        End Try

    End Sub


    Private Sub Form1_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed

        If c.ServiceUrl <> "" Then

            disconnect_ps()
            c.Disconnect()

        End If

    End Sub

    Sub get_data()

        tv_vcenter.Nodes.Clear()

        Dim bnovms As Boolean = True

        Dim doaction As [Delegate]

        If InStr(LCase(vc), "http") > 0 = False Then

            MsgBox("Virtual Center Address Must start with http:// or https://", MsgBoxStyle.Exclamation, "Invalid Format")

            txtvc.Focus()

            Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.Close()

            Exit Sub


        End If

        Dim avc As Array = Split(vc, "://")

        vcps = avc(1)

        Try

            ssessionid = us.Key

            If ssessionid = "" Then

                Dim surl As String = vc & "/sdk"

                conn = c.Connect(surl)

                us = c.Login(vcusername, vcpw)

                ssessionid = us.Key

                cmdCreateVM.Enabled = True

            End If


        Catch ex As Exception

            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical, "Error")
            pb1.Visible = False

            ssession = ""
            ssessionid = ""

            Exit Sub

        End Try

        If spssessionid = "" Then

            runspace.Open()
            spssessionid = connect_ps()

        End If

        Try


            Dim vmsHosts = c.FindEntityViews(GetType(HostSystem), Nothing, Nothing, Nothing)

            If vmsHosts Is Nothing Then

                MsgBox("Your Virtual Center Session has expired - Please Login", MsgBoxStyle.Information, "Session Expired")

                dlgvc.Show()

                Exit Sub

            End If

            Dim tvvs As TreeNode

            doaction = Sub() tvvs = tv_vcenter.Nodes.Add("Virtual Center: " & txtvc.Text, "Virtual Center: " & txtvc.Text, 3)
            tv_vcenter.Invoke(doaction)

            Dim filter As Specialized.NameValueCollection = New Specialized.NameValueCollection

            filter.Add("name", "^_avbackup")

            Dim thevm As VirtualMachine

            Dim vms = c.FindEntityViews(GetType(VirtualMachine), Nothing, filter, Nothing)

            Dim tvvms As TreeNode
            Dim tvdisks As TreeNode
            htvmhost.Clear()

            If vms IsNot Nothing Then

                If vms.Count > 0 Then

                    bnovms = False

                    For Each thevm In vms

                        Dim sthehost As String = get_host(thevm.Name)
                        htvmhost.Add(thevm.Name, sthehost)

                    Next


                End If

            Else

                bnovms = True

            End If

            If bnovms = True Then

                doaction = Sub() tvvms = tvvs.Nodes.Add("*** No Backup VM Found ***", "*** No Backup VM Found ***", 5, 5)
                tv_vcenter.Invoke(doaction)

                doaction = Sub() tvvms.BackColor = Color.Yellow
                tv_vcenter.Invoke(doaction)

            End If

            Dim tvhosts As TreeNode

            ahosts.Clear()
            abadhosts.Clear()

            For Each evbHostSystem In vmsHosts

                Dim hs = CType(evbHostSystem, HostSystem)
                ahosts.Add(hs.Name)

                Dim sstatus As String = get_host_state(hs.Name)


                Select Case sstatus

                    Case "Connected"

                        doaction = Sub() tvhosts = tvvs.Nodes.Add("OK", "vSphere Host: " & hs.Name, 4)
                        tv_vcenter.Invoke(doaction)

                        If htvmhost.ContainsValue(hs.Name) Then

                            Dim thing As DictionaryEntry

                            For Each thing In htvmhost

                                If thing.Value = hs.Name Then

                                    Dim bbackup As Boolean = False

                                    If get_snapshots(thing.Key).Count > 0 Then

                                        bbackup = True

                                        doaction = Sub() tvvms = tvhosts.Nodes.Add("Backup", "Backup VM: " & thing.Key & " *** Currently Being Backed Up ***", 5, 5)
                                        tv_vcenter.Invoke(doaction)

                                        doaction = Sub() tvvms.BackColor = Color.Yellow
                                        tv_vcenter.Invoke(doaction)

                                    Else

                                        doaction = Sub() tvvms = tvhosts.Nodes.Add("OK", "Backup VM: " & thing.Key, 5, 5)
                                        tv_vcenter.Invoke(doaction)

                                    End If

                                    For Each item In pop_disks(thing.Key)

                                        If InStr(item, thing.Key) = False Then

                                            If item = "** No AppStacks Attached **" Then

                                                doaction = Sub() tvdisks = tvvms.Nodes.Add(item, item, 6, 6)
                                                tv_vcenter.Invoke(doaction)

                                                doaction = Sub() tvdisks.BackColor = Color.Yellow
                                                tv_vcenter.Invoke(doaction)

                                            Else

                                                If InStr(LCase(item), "writable") > 0 Then

                                                    If bbackup = True Then

                                                        doaction = Sub() tvdisks = tvvms.Nodes.Add("Backup", item & " Backing Up...", 2, 2)
                                                        tv_vcenter.Invoke(doaction)

                                                        doaction = Sub() tvdisks.BackColor = Color.Yellow
                                                        tv_vcenter.Invoke(doaction)

                                                    Else

                                                        If htwritables.Count = 0 Then

                                                            doaction = Sub() tvdisks = tvvms.Nodes.Add(item, item, 2, 2)
                                                            tv_vcenter.Invoke(doaction)

                                                        Else

                                                            doaction = Sub() tvdisks = tvvms.Nodes.Add(item, htwritables(item), 2, 2)
                                                            tv_vcenter.Invoke(doaction)


                                                        End If

                                                    End If


                                                Else

                                                    If bbackup = True Then

                                                        doaction = Sub() tvdisks = tvvms.Nodes.Add(item, item & " Backing Up...", 1, 1)
                                                        tv_vcenter.Invoke(doaction)

                                                        doaction = Sub() tvdisks.BackColor = Color.Yellow
                                                        tv_vcenter.Invoke(doaction)

                                                    Else

                                                        If htas.Count = 0 Then

                                                            doaction = Sub() tvdisks = tvvms.Nodes.Add(item, item, 1, 1)
                                                            tv_vcenter.Invoke(doaction)

                                                        Else

                                                            doaction = Sub() tvdisks = tvvms.Nodes.Add(item, htas(item), 1, 1)
                                                            tv_vcenter.Invoke(doaction)

                                                        End If



                                                    End If




                                                End If

                                            End If

                                        End If
                                    Next

                                End If

                            Next

                        End If


                    Case "Disconnected"

                        ahosts.Remove(hs.Name)

                        doaction = Sub() tvhosts = tvvs.Nodes.Add("Disconnected", "vSphere Host: " & hs.Name & " ***Host Disconnected***", 4)
                        tv_vcenter.Invoke(doaction)

                        doaction = Sub() tvhosts.BackColor = Color.Yellow
                        tv_vcenter.Invoke(doaction)


                    Case "NotResponding"

                        ahosts.Remove(hs.Name)

                        doaction = Sub() tvhosts = tvvs.Nodes.Add("Not Responding", "vSphere Host: " & hs.Name & " ***Host Not Responding***", 4)
                        tv_vcenter.Invoke(doaction)

                        doaction = Sub() tvhosts.BackColor = Color.Yellow
                        tv_vcenter.Invoke(doaction)

                    Case "Maintenance"

                        ahosts.Remove(hs.Name)

                        doaction = Sub() tvhosts = tvvs.Nodes.Add("Maintenance Mode", "vSphere Host: " & hs.Name & " ***Host in Maintenance Mode***", 4)
                        tv_vcenter.Invoke(doaction)

                        doaction = Sub() tvhosts.BackColor = Color.Yellow
                        tv_vcenter.Invoke(doaction)

                End Select


            Next


        Catch ex As Exception

            If ex.HResult = "-2146233087" Then

                MsgBox(ex.Message)
                ssession = ""
                spssessionid = ""

                'get_data()


            Else


                MsgBox("Error:" & ex.Message)


            End If

        End Try

        doaction = Sub() tv_vcenter.ExpandAll()
        tv_vcenter.Invoke(doaction)
        pb1.Visible = False

    End Sub


    Function get_host(ByVal svm As String) As String

        Dim pipeline As Pipeline = runspace.CreatePipeline

        pipeline.Commands.AddScript("Get-VMHost -VM """ & svm & """")

        Dim thereturnValue As Collection(Of PSObject) = pipeline.Invoke

        Dim sreturn As String = "The following VM's were removed: " & vbCrLf

        For Each result As PSObject In thereturnValue

            Return result.Properties("Name").Value.ToString

        Next

        Return sreturn


    End Function

    Function get_host_state(ByVal shost As String) As String

        Dim pipeline As Pipeline = runspace.CreatePipeline

        pipeline.Commands.AddScript("Get-VMHost -Name """ & shost & """")

        Dim thereturnValue As Collection(Of PSObject) = pipeline.Invoke

        For Each result As PSObject In thereturnValue

            Return result.Properties("State").Value.ToString

        Next

        Return "Nothing"

    End Function

    Function get_datastores(ByVal shost As String) As ArrayList

        Dim ads As New ArrayList

        Dim pipeline As Pipeline = runspace.CreatePipeline

        pipeline.Commands.AddScript("Get-Datastore -VMHost """ & shost & """")

        Dim thereturnValue As Collection(Of PSObject) = pipeline.Invoke

        Dim sreturn As String = "The following VM's were removed: " & vbCrLf

        For Each result As PSObject In thereturnValue

            ads.Add(result.Properties("Name").Value.ToString)

        Next

        Return ads

    End Function

    Function get_snapshots(ByVal svm As String) As ArrayList

        Dim ads As New ArrayList

        Dim pipeline As Pipeline = runspace.CreatePipeline

        pipeline.Commands.AddScript("Get-Snapshot -VM """ & svm & """")

        Dim thereturnValue As Collection(Of PSObject) = pipeline.Invoke

        Dim sreturn As String = "The following VM's were removed: " & vbCrLf

        For Each result As PSObject In thereturnValue

            ads.Add(result.Properties("Name").Value.ToString)

        Next

        Return ads

    End Function

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = False

        If reg_key_exists("Software\VMware Flings\App Volumes Backup") = False Then

            Try
                Registry.CurrentUser.CreateSubKey("Software\VMware Flings\App Volumes Backup")

            Catch ex As Exception

                MessageBox.Show("Error: " & ex.Message)

            End Try

        End If

        If Reg_Read("Software\VMware Flings\App Volumes Backup", "AV Manager") <> "NO_VALUE" Then

            txtAVManager.Text = Reg_Read("Software\VMware Flings\App Volumes Backup", "AV Manager")

            savmanager = txtAVManager.Text

        Else


            txtAVManager.Text = "http://<App Volumes Manager>"

        End If

        If Reg_Read("Software\VMware Flings\App Volumes Backup", "vCenter") <> "NO_VALUE" Then

            txtvc.Text = Reg_Read("Software\VMware Flings\App Volumes Backup", "vCenter")

            vc = txtvc.Text

        Else

            txtvc.Text = "https://<Virtual Center>"

        End If

        If Reg_Read("Software\VMware Flings\App Volumes Backup", "AV_Username") <> "NO_VALUE" Then

            dlgav.txtusername.Text = Reg_Read("Software\VMware Flings\App Volumes Backup", "AV_Username")

        Else

            dlgav.txtusername.Text = "domain\username"

        End If


        If Reg_Read("Software\VMware Flings\App Volumes Backup", "VC_Username") <> "NO_VALUE" Then

            dlgvc.txtusername.Text = Reg_Read("Software\VMware Flings\App Volumes Backup", "VC_Username")

        Else

            dlgvc.txtusername.Text = "domain\username"

        End If


        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls Or SecurityProtocolType.Ssl3 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls12


    End Sub

    Private Sub txtvc_GotFocus(sender As Object, e As EventArgs) Handles txtvc.GotFocus

        If vc = "" Then

            If txtvc.Text = "https://<Virtual Center>" Then


                txtvc.Text = "https://"

            End If


        End If



    End Sub

    Private Sub txtvc_TextChanged(sender As Object, e As EventArgs) Handles txtvc.TextChanged

        Reg_Write("Software\VMware Flings\App Volumes Backup", "vCenter", txtvc.Text)
        vc = txtvc.Text



    End Sub



    Private Sub txtAVManager_GotFocus(sender As Object, e As EventArgs) Handles txtAVManager.GotFocus

        If savmanager = "" Then

            If txtAVManager.Text = "http://<App Volumes Manager>" Then

                txtAVManager.Text = "http://"

            End If


        End If


    End Sub

    Private Sub txtAVManager_TextChanged(sender As Object, e As EventArgs) Handles txtAVManager.TextChanged

        Reg_Write("Software\VMware Flings\App Volumes Backup", "AV Manager", txtAVManager.Text)
        savmanager = txtAVManager.Text

    End Sub



    Private Sub ShowInstalledApplicationsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowInstalledApplicationsToolStripMenuItem.Click

        If lstAS.SelectedItems.Count > 1 Then

            MsgBox("You can't view installed applications for more than one AppStack at a time", MsgBoxStyle.OkOnly, "Too many AppStacks Selected")

            Exit Sub

        Else

            For Each item In lstAS.SelectedItems

                'If MsgBox("Are you sure you want to show the applications installed in the " & item.text & " AppStack?", MsgBoxStyle.YesNo, "Show Intalled Apps?") = MsgBoxResult.Yes Then

                Dim sid As String = item.SubItems(4).Text()

                get_as_apps(sid)

                'get_apps(htas(item.text))

                dlgApps.Text = "Applications Installed in " & item.text & " AppStack"
                dlgApps.ShowDialog()

                ' End If


            Next


        End If


    End Sub

    Private Sub cmdRemoveDrives_Click(sender As Object, e As EventArgs) Handles cmdRemoveDrives.Click

        Dim selecteddata As List(Of TreeNode) = GetChildren(tv_vcenter)

        Dim item As TreeNode

        Dim squestion As String = "Are you sure you want to detach the following AppStacks?" & vbCrLf & vbCrLf

        For Each item In selecteddata

            If item.Checked = True And item.Level = "3" Then

                squestion = squestion & "*-" & item.Text & vbCrLf

            End If


        Next

        If MsgBox(squestion, MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Detach AppStacks") = MsgBoxResult.Yes Then

            t = New System.Threading.Thread(AddressOf remove_drives)

            t.Start()

        End If


    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click

        If MsgBox("Are you sure you want to Exit?", MsgBoxStyle.Question + vbYesNo, "Exit?") = vbYes Then

            If c.ServiceUrl <> "" Then

                disconnect_ps()
                c.Disconnect()

            End If

            End

        End If
    End Sub


    Private Sub ShowAppStackLocationsToolStripMenuItem_Click(sender As Object, e As EventArgs)




    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click

        MsgBox("AppStack Backup Utility" & vbCrLf & "Version: " & Me.ProductVersion & vbCrLf & "Chris Halstead - @chrisdhalstead" & vbCrLf & "Copyright © 2016", MsgBoxStyle.Information, "About...")

    End Sub

    Private Sub MenuStrip1_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles MenuStrip1.ItemClicked

    End Sub
End Class




