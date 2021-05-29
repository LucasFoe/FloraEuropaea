Namespace My

    ' The following events are availble for MyApplication:
    ' 
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.

    Partial Friend Class MyApplication


        Private Sub MyApplication_Startup(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup
            LoadCmdLineArgs()
            StartMain.Main()
        End Sub

        Private Sub MyApplication_StartupNextInstance(sender As Object, e As Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs) Handles Me.StartupNextInstance
            LoadCmdLineArgs()
            StartMain.AnaCmd()
        End Sub

        Private Sub MyApplication_UnhandledException(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
            'Global.GUIFE.SplashScreen1.btMsg.Text = "Error durign start of GUIFE: " & e.ToString
            'Global.GUIFE.SplashScreen1.btMsg.Visible = True
            'Global.GUIFE.SplashScreen1.btMsg.Enabled = True   
            MessageBox.Show(Me.SplashScreen, "Error durign start of GUIFE: " & e.ToString, "Exit GUIFE", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            e.ExitApplication = True
        End Sub

        'Protected Overrides Function OnInitialize(ByVal commandLineArgs As System.Collections.ObjectModel.ReadOnlyCollection(Of String)) As Boolean
        'Me.MinimumSplashScreenDisplayTime = 5000
        'Return MyBase.OnInitialize(commandLineArgs)
        'End Function

    End Class

End Namespace

