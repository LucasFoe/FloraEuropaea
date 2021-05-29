Imports System.Data.OleDb
Imports System.IO
Imports System.Threading
Imports LfUtilities

Module StartMain
    Public sett As Setting
    Public ucList As UserCmdList
    Public ht As HandleText
    Public pi As PlantIndex
    Public gl As Glossary
    Public kc As KeyCollection
    Public log As Logger

    Public connStrIndex As String
    Public connStrFamilyKey As String
    Public connStrGeneraKey As String
    Public connStrMainKey As String

    Public connIndex As OleDbConnection

    Public connFamilyKey As OleDbConnection
    Public schFamilyKey As DataTable
    Public dvFamilyKey As DataView

    Public connGeneraKey As OleDbConnection
    Public schGeneraKey As DataTable
    Public dvGeneraKey As DataView

    Public connMainKey As OleDbConnection
    Public schMainKey As DataTable
    Public dvMainKey As DataView

    Public CmdLineArgs As New List(Of String)
    Public CmdSearchGenus As String
    Public CmdSearchSpecies As String

    Public win32 As New NativeMethods()

    Public Sub DisposeIfNotNull(Of T As IDisposable)(dispobj As T)
        If dispobj IsNot Nothing Then
            dispobj.Dispose()
        End If
    End Sub

    Public Sub LoadCmdLineArgs()
        If My.Application.CommandLineArgs.Count > 0 Then
            CmdLineArgs.Clear()
            For Each c As String In My.Application.CommandLineArgs
                CmdLineArgs.Add(c)
            Next
        End If
    End Sub

    Sub InitConnection()

        Dim connStrTpl As String, ds As String
        connStrTpl = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source='$$DS$';"

        ds = lf_filename(sett.DBDir, "INDEX.MDB")
        connStrIndex = Replace(connStrTpl, "$$DS$", ds)
        ds = lf_filename(sett.DBDir, "families.mdb")
        connStrFamilyKey = Replace(connStrTpl, "$$DS$", ds)
        ds = lf_filename(sett.DBDir, "genera.mdb")
        connStrGeneraKey = Replace(connStrTpl, "$$DS$", ds)
        ds = lf_filename(sett.DBDir, "MYDB.mdb")
        connStrMainKey = Replace(connStrTpl, "$$DS$", ds)

        connIndex = New OleDbConnection(connStrIndex)

        connFamilyKey = New OleDbConnection(connStrFamilyKey)
        connGeneraKey = New OleDbConnection(connStrGeneraKey)
        connMainKey = New OleDbConnection(connStrMainKey)

        connFamilyKey.Open()
        schFamilyKey = connFamilyKey.GetSchema("Tables")
        connFamilyKey.Close()
        dvFamilyKey = schFamilyKey.DefaultView
        dvFamilyKey.RowFilter = "TABLE_TYPE = 'TABLE'"

        connGeneraKey.Open()
        schGeneraKey = connGeneraKey.GetSchema("Tables")
        connGeneraKey.Close()
        dvGeneraKey = schGeneraKey.DefaultView
        dvGeneraKey.RowFilter = "TABLE_TYPE = 'TABLE'"

        connMainKey.Open()
        schMainKey = connMainKey.GetSchema("Tables")
        connMainKey.Close()
        dvMainKey = schMainKey.DefaultView
        dvMainKey.RowFilter = "TABLE_TYPE = 'TABLE'"
    End Sub

    Sub InitKey()
        kc = New KeyCollection
        kc.AddKeySet(connMainKey, dvMainKey, "ABOVE")
        kc.AddKeySet(connFamilyKey, dvFamilyKey, "FAM")
        kc.AddKeySet(connGeneraKey, dvGeneraKey, "GEN")
    End Sub

    Public Function GetClip() As String
        Return Clipboard.GetText()
    End Function

    Public Sub SetClip(ByVal t As String)
        Clipboard.SetText(t)
    End Sub

    Sub ExitApplication()
        Try
            FEForma1.Close()
        Catch ex As Exception
        End Try
        Try
            FEForma2.Close()
        Catch ex As Exception
        End Try
        Try
            FEForma3.Close()
        Catch ex As Exception
        End Try
        Try
            FEForma4.Close()
        Catch ex As Exception
        End Try
        Application.Exit()
    End Sub

    ' Catch a ThreadException event.
    Private Sub app_ThreadException(ByVal sender As Object, ByVal e As ThreadExceptionEventArgs)
        Console.WriteLine("Caught unhandled exception")
    End Sub

    Sub initglossary()
        If sett.FloraExeStatusOK Then
            ' Glossary aus flora.exe auslesen

            gl = New Glossary
            Dim gel As List(Of GlossaryElement)
            Try

                gel = Glossary.Load()
                gl.GelProperty = gel
            Catch ex As Exception
            End Try
            If gl Is Nothing Then
                gl = New Glossary
            End If

            If Not gl.IsFilled Then
                gl.LoadData()
                gl.Save()
            End If

        End If
    End Sub

    Public Sub StartCmd(ByVal cmd As String, Optional ByVal cmdLinePar As String = "",
                        Optional ByVal wrkdir As String = "")
        If Not cmdLinePar.StartsWith("""") Then
            cmdLinePar = """" & cmdLinePar & """"
        End If
        Try
            Dim psInfo As ProcessStartInfo
            If Util.IsEmpty(cmdLinePar) Then
                psInfo = New ProcessStartInfo(cmd)
            Else
                psInfo = New ProcessStartInfo(cmd, cmdLinePar)
            End If
            If Not Util.IsEmpty(wrkdir) Then
                psInfo.WorkingDirectory = wrkdir
            End If
            psInfo.WindowStyle =
                ProcessWindowStyle.Normal
            Dim myProcess As Process =
                    Process.Start(psInfo)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub AnaCmd()

        'Agrostis schleicheri
        CmdSearchGenus = ""
        CmdSearchSpecies = ""

        ' Search genus (for ex Agrostis)
        If CmdLineArgs.Count > 0 Then
            Dim w() As String
            w = Split(CmdLineArgs(0), " ")
            If w.Length > 1 Then
                CmdSearchGenus = w(0)
                CmdSearchSpecies = w(1)
            Else
                CmdSearchGenus = CmdLineArgs(0)
            End If

        End If

        ' Search Species (for ex. schleicheri)
        If CmdLineArgs.Count > 1 And CmdSearchSpecies.Length = 0 Then
            CmdSearchSpecies = CmdLineArgs(1)
        End If
    End Sub

    Sub MsgBoxOnTop(text As String)
        MessageBox.Show(New Form() With {
              .TopMost = True
          }, text, "GUIFE startup warning")
    End Sub

    Sub CheckGaisVol()

        If Not File.Exists(sett.GaisExe) Then
            MsgBoxOnTop("Parameter GAISEXE in flora.ini: " & sett.GaisExe & " not found! (search not working)")
        End If
        Dim f1 = Path.Combine(sett.GaisVol, ".gais_inc")
        If Not File.Exists(f1) Then
            MsgBoxOnTop(f1 & " not found! (search not working)")
        Else
            Dim t1 = File.ReadAllText(f1)
            t1 = Util.AlltrimSet(t1, vbCrLf & " ")
            If Not Directory.Exists(t1) Then
                MsgBoxOnTop(t1 & " not found! (search not working)")
            End If
        End If
        Dim f2 = Path.Combine(sett.GaisVol, ".gais_directorylist")
        If Not File.Exists(f2) Then
            MsgBoxOnTop(f2 & " not found! (search not working)")
        Else
            Dim t1 = File.ReadAllText(f1)
            t1 = Util.AlltrimSet(t1, vbCrLf & " ")
            If Not Directory.Exists(t1) Then
                MsgBoxOnTop(t1 & " not found! (search not working)")
            End If
        End If

    End Sub

    Sub Main()

        AddHandler Application.ThreadException, AddressOf app_ThreadException

        Application.OnThreadException(New InvalidDataException("There"))

        AnaCmd()

        ' Log File initialisieren
        log = New Logger(True)

        ' Settings from ini file
        sett = New Setting()

        If Not sett.SettingStatusOk Then
            Try
                FEForma4.Text = "Error in settings!"
                FEForma4.ShowDialog()
            Catch ex As Exception
            End Try
            FEForma4.Activate()
            sett = New Setting()
        End If

        If Not sett.SettingStatusOk Then
            Try
                FEForma4.Text = "Still error in settings!"
                FEForma4.ShowDialog()
            Catch ex As Exception
            End Try
            FEForma4.Activate()
            sett = New Setting()
        End If
        FEForma4.Text = "Settings"

        If Not sett.SettingStatusOk Then
            MsgBoxOnTop("Still errors in settings!!")
        End If

        CheckGaisVol()

        If sett.TextStatusOk Then
            ' Text handler oeffnen
            ht = New HandleText(sett.TextDir)
        End If

        ucList = New UserCmdList(sett.UserCmd)

        initglossary()

        If sett.DBStatusOk Then
            ' DB Connections definieren
            InitConnection()
            ' Key initialisieren
            InitKey()
            ' Index Informationen aus DB übernehmen
            pi = New PlantIndex(connIndex)
        End If

        If StartMain.CmdSearchGenus.Length > 0 Or StartMain.CmdSearchSpecies.Length > 0 Then
            FEForma1.SetStartFlt(StartMain.CmdSearchGenus, StartMain.CmdSearchSpecies)
        End If
    End Sub

End Module
