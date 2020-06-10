Namespace My
    ' The following events are available for MyApplication:
    ' 
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication

        Private Sub MyApplication_UnhandledException(ByVal sender As Object, _
                                                     ByVal e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs _
                                                     ) Handles Me.UnhandledException

            MessageBox.Show("Make sure you have VMware PowerCli 6.x Installed")
            MessageBox.Show("Application crashed due to the following unhandled exception. " + e.Exception.ToString())



        End Sub


    End Class


End Namespace

