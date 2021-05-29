Imports System
Imports System.Collections
Imports System.IO
Imports System.Reflection
Imports System.Configuration
Imports System.ComponentModel
Imports System.Collections.Generic
Imports System.Text
Imports System.Windows.Forms
Imports System.Windows.Forms.Design
Imports System.Drawing.Design
Imports LfUtilities

Public Class UIDirectoryNameEditor
    Inherits System.Drawing.Design.UITypeEditor

    Public Overloads Overrides Function GetEditStyle(ByVal context As ITypeDescriptorContext) As UITypeEditorEditStyle
        If context IsNot Nothing Then
            Return UITypeEditorEditStyle.Modal
        End If

        Return UITypeEditorEditStyle.None
    End Function

    <RefreshProperties(RefreshProperties.All)> _
    Public Overloads Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) _
        As Object
        Dim fd As System.Windows.Forms.FolderBrowserDialog

        If context Is Nothing OrElse provider Is Nothing OrElse context.Instance Is Nothing Then
            Return MyBase.EditValue(provider, value)
        End If

        fd = New System.Windows.Forms.FolderBrowserDialog()

        If fd.ShowDialog() = DialogResult.OK Then
            value = fd.SelectedPath
        End If
        Return value

    End Function
End Class

Public Class UIFileNameEditor
    Inherits System.Drawing.Design.UITypeEditor

    Public Overloads Overrides Function GetEditStyle(ByVal context As ITypeDescriptorContext) As UITypeEditorEditStyle
        If context IsNot Nothing Then
            Return UITypeEditorEditStyle.Modal
        End If

        Return UITypeEditorEditStyle.None
    End Function

    <RefreshProperties(RefreshProperties.All)> _
    Public Overloads Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) _
        As Object
        Dim fd As FileDialog
        Dim MyFolderBrowser As New System.Windows.Forms.FolderBrowserDialog

        If context Is Nothing OrElse provider Is Nothing OrElse context.Instance Is Nothing Then
            Return MyBase.EditValue(provider, value)
        End If

        fd = New System.Windows.Forms.OpenFileDialog()
        fd.CheckFileExists = True
        fd.CheckPathExists = True
        fd.Title = "Select Filename"
        fd.FileName = TryCast(value, String)
        fd.Filter = "All Files (*.*)|*.*"

        If fd.ShowDialog() = DialogResult.OK Then
            If System.IO.File.Exists(fd.FileName) Then
                value = fd.FileName
            End If
        End If
        Return value

    End Function
End Class

'<AttributeUsage(AttributeTargets.Property)> _
'Public Class SaveFileAttribute
'Inherits Attribute
'End Class

' Klasse zur Verwaltung von Programmeinstellungen (Settings)
' Metadaten dazu werden aus einem Textfile gelesen
Public Class Setting
    Inherits UITypeEditor

    Private options As Collection
    Private metasyn As Collection
    Private linktaxa As Collection
    Private comment As Collection

    Private mtxtdir As String = ""
    Private mhomedir As String = ""
    Private mdatadir As String = ""
    Private mdbdir As String = ""
    Private mgaisvol As String = ""
    Private minifile As String = "flora.ini"
    Private mfloraexe As String = "flora.exe"
    Private mGaisExe As String = "gais.exe"
    Private moutfile As String = "found.lst"
    Private musercmd As String = "user.ini"
    Private mIsDropDown As Boolean
    Private mCmdTpl As String = ""
    Private mUnchangeableRest As List(Of String)
    Private f As LFFile

    Private mSettingStatusOk As Boolean = False
    Private mTextStatusOK As Boolean = False
    Private mDBStatusOK As Boolean = False
    Private mSearchStatusOK As Boolean = False
    Private mFloraExeStatusOK As Boolean = False
    Private mDelCache As Boolean = False

    <DisplayName("DeleteCache"), _
        DescriptionAttribute("Cache will be deleteted on next start of application"), _
        Category(" Startup")> _
    Public Property DeleteCache() As Boolean
        Get
            Return mDelCache
        End Get
        Set(ByVal value As Boolean)
            mDelCache = value
        End Set
    End Property

    <DisplayName("Status of settings is OK"), _
        DescriptionAttribute("Application not fully active if status is not OK."), _
        Category(" Status")> _
    Public ReadOnly Property SettingStatusOk() As Boolean
        Get
            Return mSettingStatusOk
        End Get

    End Property

    <DisplayName("Status of text data is OK"), _
        DescriptionAttribute("Status text data configuration. No access to text data if status is not OK."), _
        Category(" Status")> _
    Public ReadOnly Property TextStatusOk() As Boolean
        Get
            Return mTextStatusOK
        End Get
    End Property

    <DisplayName("Status of search configuration is OK"), _
        DescriptionAttribute("Search not active if status is not OK."), _
        Category(" Status")> _
    Public ReadOnly Property SearchStatusOK() As Boolean
        Get
            Return mSearchStatusOK
        End Get
    End Property

    <DisplayName("Status of DB access is OK"), _
        DescriptionAttribute("Keys and indexes not active if status is not OK."), _
        Category(" Status")> _
    Public ReadOnly Property DBStatusOk() As Boolean
        Get
            Return mDBStatusOK
        End Get
    End Property

    <DisplayName("Status of access to flora.exe is OK"), _
        DescriptionAttribute("Glossary not active."), _
        Category(" Status")> _
    Public ReadOnly Property FloraExeStatusOK() As Boolean
        Get
            Return mFloraExeStatusOK
        End Get
    End Property

    <DisplayName("Home Directory"), _
        DescriptionAttribute("Working directory of application"), _
        Category("Directories")> _
    Public ReadOnly Property HomeDir() As String
        Get
            Return mhomedir
        End Get
    End Property

    <DisplayName("DB Directory"), _
        Editor(GetType(UIDirectoryNameEditor), GetType(UITypeEditor)), _
        DescriptionAttribute("Directory containing the MDB files (format: MS Access)"), _
        Category("Directories")> _
    Public Property DBDir() As String
        Get
            Return mdbdir
        End Get
        Set(ByVal Value As String)
            mdbdir = Value
        End Set
    End Property

    <DisplayName("Text Directory"), _
        Editor(GetType(UIDirectoryNameEditor), GetType(UITypeEditor)), _
        DescriptionAttribute("Directory containing the text files (format: Richtext)"), _
        Category("Directories")> _
    Public Property TextDir() As String
        Get
            Return mtxtdir
        End Get
        Set(ByVal Value As String)
            mtxtdir = Value
        End Set
    End Property

    <DisplayName("Data Directory"), _
        Editor(GetType(UIDirectoryNameEditor), GetType(UITypeEditor)), _
        DescriptionAttribute("Directory containing additional data"), _
        Category("Directories")> _
    Public Property DataDir() As String
        Get
            Return mdatadir
        End Get
        Set(ByVal Value As String)
            mdatadir = Value
        End Set
    End Property

    <DisplayName("Index Directory"), _
        Editor(GetType(UIDirectoryNameEditor), GetType(UITypeEditor)), _
        DescriptionAttribute("Directory containing indexes for full text search"), _
        Category("Directories")> _
    Public Property GaisVol() As String
        Get
            Return mgaisvol
        End Get
        Set(ByVal Value As String)
            mgaisvol = Value
        End Set
    End Property

    <DisplayName("INI File"), _
        DescriptionAttribute("File with initial settings (has to be in home directory)"), _
        Category("Files")> _
    Public ReadOnly Property IniFile() As String
        Get
            Return minifile
        End Get
    End Property

    <DisplayName("Path of original exe"), _
        Editor(GetType(UIFileNameEditor), GetType(UITypeEditor)), _
        DescriptionAttribute("Path of exe file of original GUI"), _
        Category("Files")> _
    Public Property FloraExe() As String
        Get
            Return mfloraexe
        End Get
        Set(ByVal Value As String)
            mfloraexe = Value
        End Set
    End Property

    <DisplayName("Path of user commands"), _
        Editor(GetType(UIFileNameEditor), GetType(UITypeEditor)), _
        DescriptionAttribute("Path of file containing user commands"), _
        Category("Files")> _
    Public Property UserCmd() As String
        Get
            Return musercmd
        End Get
        Set(ByVal Value As String)
            musercmd = Value
        End Set
    End Property

    <DisplayName("Out File of full text search"), _
        Editor(GetType(UIFileNameEditor), GetType(UITypeEditor)), _
        DescriptionAttribute("Workfile with result of full text search"), _
        Category("Files")> _
    Public Property OutFile() As String
        Get
            Return moutfile
        End Get
        Set(ByVal Value As String)
            moutfile = Value
        End Set
    End Property

    <DisplayName("Path of search program"), _
        Editor(GetType(UIFileNameEditor), GetType(UITypeEditor)), _
        DescriptionAttribute("Path of program used for full text search"), _
        Category("Files")> _
    Public Property GaisExe() As String
        Get
            Return mGaisExe
        End Get
        Set(ByVal Value As String)
            mGaisExe = Value
        End Set
    End Property

    '<DisplayName("Cmd for full text search"), _
    '    DescriptionAttribute("Command line template for full text search"), _
    '    Category("Files")> _
    <Browsable(False)> _
    Public Property CmdTpl() As String
        Get
            Return mCmdTpl
        End Get
        Set(ByVal Value As String)
            mCmdTpl = Value
        End Set
    End Property

    <Browsable(False)> _
    Public Overrides ReadOnly Property IsDropDownResizable() As Boolean
        Get
            Return mIsDropDown
        End Get
    End Property

    Sub New()
        Me.options = New Collection()
        Me.metasyn = New Collection()
        Me.linktaxa = New Collection()
        Me.comment = New Collection()

        Me.mhomedir = GetApplicationFolderName()

        If IniFile.Length > 0 Then
            Me.minifile = IniFile
        End If
        Try
            readsettings()
            CheckSettings()
        Catch ex As Exception
            MsgBox(ex.Message)
            mSettingStatusOk = False
        End Try

    End Sub

    Private Function cleanopt(ByVal o As String) As String
        Dim r As String = o
        r = Replace(r, "(error)", "")
        r = Replace(r, "(Error)", "")
        r = r.Trim
        Return r
    End Function

    Private Sub readsettings()

        ' Setting haben folgendes Format:
        '    Key=Value
        ' Zeilen beginnend mit // oder * sind Kommentare

        Dim line As String
        Dim key As String
        Dim opt As String
        Dim fn As String
        Dim st As StreamReader
        Dim optionflag As Boolean = True
        Dim synflag As Boolean = False
        Dim linktaxaflag As Boolean = False
        Dim cmt As String

        mUnchangeableRest = New List(Of String)

        fn = Path.Combine(Me.HomeDir, minifile)
        Dim restflag As Boolean = False

        Try
            st = New StreamReader(fn)
            cmt = ""
            While (st.Peek() >= 0)

                line = st.ReadLine().Trim

                If Util.Match(line, "#\s*CFG\s*") Then
                    restflag = True
                End If

                If restflag Then
                    mUnchangeableRest.Add(line)
                End If

                If Util.Left(line, 1) = "#" Then
                    ' Steuerbefehle
                    cmt = cmt & line & "הה"
                    line = line.ToUpper
                    If line.Contains("META:") And line.Contains("SYN") Then
                        synflag = True
                        optionflag = False
                    ElseIf line.Contains("META:") And line.Contains("LINK") Then
                        linktaxaflag = True
                        optionflag = False
                        synflag = False
                    End If
                Else

                    If line.TrimStart().StartsWith("*") Or line.TrimStart().StartsWith("//") Then

                        ' Kommentare ueberlesen
                        cmt = cmt & line & "הה"
                    Else

                        '# DIR
                        'TEXTDIR=c:\vbnet\Visual Studio Projects\FloraEuropaea\DATA\gaisrtf
                        'DATADIR=c:\vbnet\Visual Studio Projects\FloraEuropaea\DATA\
                        'GAISVOL=c:\vbnet\Visual Studio Projects\FloraEuropaea\DATA\gaisvol
                        'MDBDIR=c:\vbnet\Visual Studio Projects\FloraEuropaea\DATA
                        'HOMEDIR=c:\vbnet\Visual Studio Projects\FloraEuropaea\FloraEuropaea\bin\Debug
                        '# FILE
                        '                        LogFile = FLORA.LOG
                        '                        FLORAEXE = flora.exe
                        'GAISEXE=c:\vbnet\Visual Studio Projects\FloraEuropaea\GAIS.EXE
                        'OUTFILE=c:\vbnet\Visual Studio Projects\FloraEuropaea\found.lst

                        If linktaxaflag Then
                            key = GetCmdKey(line, ",")
                            opt = GetCmdOption(line, ",")
                        Else
                            key = GetCmdKey(line, "=")
                            opt = GetCmdOption(line, "=")
                        End If

                        If cmt.Length > 0 Then
                            Me.AddCmt(key, cmt)
                            cmt = ""
                        End If

                        If key = "TEXTDIR" Then
                            Me.TextDir = cleanopt(opt)
                        ElseIf key = "DATADIR" Then
                            Me.DataDir = cleanopt(opt)
                        ElseIf key = "MDBDIR" Then
                            Me.DBDir = cleanopt(opt)
                        ElseIf key = "DBDIR" Then
                            Me.DBDir = cleanopt(opt)
                        ElseIf key = "GAISVOL" Then
                            Me.GaisVol = cleanopt(opt)
                        ElseIf key = "GAISEXE" Then
                            Me.GaisExe = cleanopt(opt)
                        ElseIf key = "FLORAEXE" Then
                            Me.FloraExe = cleanopt(opt)
                        ElseIf key = "USERCMD" Then
                            Me.UserCmd = cleanopt(opt)
                        ElseIf key = "CMDTPL" Then
                            Me.CmdTpl = opt
                        ElseIf key = "DELCACHE" Then
                            Me.DeleteCache = opt
                        Else
                            If optionflag Then
                                AddOption(key, opt)
                            ElseIf synflag Then
                                AddMetaSyn(key, opt)
                            ElseIf linktaxaflag Then
                                AddLinkTaxa(key, opt)
                            End If
                        End If
                    End If
                End If
            End While
            st.Close()

        Catch e As Exception
            Console.WriteLine("The process failed: {0}", e.ToString())
        End Try

    End Sub

    Public Sub CheckSettings()
        Dim msg As New List(Of String)
        Dim f As FileInfo
        Dim oldName As String
        Dim corrflag As Boolean = False

        mSettingStatusOk = True
        mTextStatusOK = True
        mDBStatusOK = True
        mSearchStatusOK = True
        mFloraExeStatusOK = True

        ' Check TEXTDIR
        If lf_fileExists2(Me.TextDir, "A1100.rtf") < 0 Then
            oldName = Me.TextDir
            msg.Add("TextDir /" & oldName & "/ does not exist!")
            f = lf_FindFileInfo(Me.TextDir, "A1100.rtf")
            If f Is Nothing Then
                f = lf_FindFileInfo("c:\GAISRTF", "A1100.rtf")
                If f Is Nothing Then
                    f = lf_FindFileInfo(Me.HomeDir, "A1100.rtf")
                End If
                If f Is Nothing Then
                    mSettingStatusOk = False
                    mTextStatusOK = False
                    Me.TextDir = Me.TextDir & " (error)"
                Else
                    Me.TextDir = f.DirectoryName
                    msg.Add("TEXTDIR corrected: /" & oldName & "/ -> /" & Me.TextDir & "/")
                    corrflag = True
                End If
            End If
        End If
        ' Check DBDIR
        If lf_fileExists2(Me.DBDir, "families.mdb") < 0 Then
            oldName = Me.DBDir
            msg.Add("DBDir /" & oldName & "/ does not exist!")
            f = lf_FindFileInfo(Me.DBDir, "families.mdb")
            If f Is Nothing Then
                f = lf_FindFileInfo(Me.HomeDir, "families.mdb")
                If f Is Nothing Then
                    f = lf_FindFileInfo("c:\floracd", "families.mdb")
                End If
            End If
            If f Is Nothing Then
                mSettingStatusOk = False
                mDBStatusOK = False
                Me.DBDir = Me.DBDir & " (error)"
            Else
                Me.DBDir = f.DirectoryName
                msg.Add("DBDIR corrected: /" & oldName & "/ -> /" & Me.DBDir & "/")
                corrflag = True
            End If
        End If
        ' Check GAISVOL
        If lf_fileExists2(Me.GaisVol, ".gais_inc") < 0 Then
            oldName = Me.GaisVol
            msg.Add("GaisVol /" & Me.GaisVol & "/ does not exist!")
            f = lf_FindFileInfo(Me.GaisVol, ".gais_inc")
            If f Is Nothing Then
                f = lf_FindFileInfo("c:\GAISVOL", ".gais_inc")
                If f Is Nothing Then
                    f = lf_FindFileInfo(Me.HomeDir, ".gais_inc")
                End If
            End If
            If f Is Nothing Then
                mSettingStatusOk = False
                mSearchStatusOK = False
                Me.GaisVol = Me.GaisVol & " (error)"
            Else
                Me.GaisVol = f.DirectoryName
                msg.Add("GAISVOL corrected: /" & oldName & "/ -> /" & Me.GaisVol & "/")
                corrflag = True
            End If
        End If
        ' Check FLORAEXE
        If lf_fileExists(Me.FloraExe) < 0 Then
            If lf_fileExists2(Me.HomeDir, Me.FloraExe) < 0 Then
                msg.Add("FloraExe /" & Me.FloraExe & "/ does not exist")
                f = lf_FindFileInfo("c:\floracd", "flora.exe")
                If f Is Nothing Then
                    If Me.DataDir.Length > 10 Then
                        f = lf_FindFileInfo(Me.DataDir, Me.FloraExe)
                    End If
                End If
                If f Is Nothing Then
                    mSettingStatusOk = False
                    mFloraExeStatusOK = False
                    Me.FloraExe = Me.FloraExe & " (error)"
                Else
                    Me.FloraExe = f.FullName
                    corrflag = True
                    msg.Add("FLORAEXE corrected to " & Me.FloraExe)
                End If

            End If
        End If
        ' Check GAISEXE
        If lf_fileExists(Me.GaisExe) < 0 Then
            If lf_fileExists2(Me.HomeDir, Me.GaisExe) < 0 Then
                msg.Add("GaisExe /" & Me.GaisExe & "/ does not exist")
                f = lf_FindFileInfo("c:\floracd", "gais.exe")
                If f Is Nothing Then
                    mSettingStatusOk = False
                    mSearchStatusOK = False
                    Me.GaisExe = Me.GaisExe & " (error)"
                Else
                    Me.GaisExe = f.FullName
                    msg.Add("GAISEXE corrected to " & Me.GaisExe)
                    corrflag = True
                End If
            End If
        End If
        If msg.Count > 0 Then
            log.WriteLog(msg)
        End If
        If corrflag Then
            Me.SaveSettings()
            MessageBox.Show("Settings in flora.ini has been corrected.")
        End If
    End Sub

    ' Return Key / KEY=OPTION
    Public Function GetCmdKey(ByVal c As String, ByVal optdel As String) As String
        Dim returnValue As String
        Dim p As Integer = 0
        p = InStr(1, c, optdel)

        If p <= 0 Then
            returnValue = c
        Else
            returnValue = Left(c, p - 1)
        End If
        Return returnValue
    End Function

    ' Return Option / KEY=OPTION
    Public Function GetCmdOption(ByVal c As String, ByVal optdel As String) As String
        Dim returnValue As String
        Dim p As Integer = 0
        p = InStr(1, c, optdel)
        If p <= 0 Then
            returnValue = ""
        Else
            returnValue = Mid$(c, p + 1)
        End If
        Return returnValue
    End Function

    ' Verzeichnis von aktueller Application ermitteln
    Public Function GetApplicationFolderName() As String
        ' FileInfo-Objekt für die Datei erzeugen, die die Eintritts-Assembly speichert
        Dim fi As FileInfo = New FileInfo(Assembly.GetEntryAssembly().Location)
        ' Den Pfad des Ordners der Datei zurckgeben
        Return fi.DirectoryName
    End Function

    Public Sub AddOption(ByVal k As String, ByVal o As String)

        k = k.Trim.ToUpper
        o = o.Trim.ToUpper
        Try
            Me.options.Add(o, k)
        Catch ex As Exception
            Me.options.Remove(k)
            Me.options.Add(o, k)
        End Try

    End Sub

    Public Function GetOption(ByVal k As String) As String
        Dim r As String
        Try
            k = k.Trim.ToUpper
            r = Me.options.Item(k)
        Catch ex As Exception
            r = ""
        End Try
        Return r
    End Function

    Public Sub AddMetaSyn(ByVal k As String, ByVal o As String)

        k = k.Trim.ToUpper
        o = o.Trim.ToUpper
        Try
            Me.metasyn.Add(o, k)
        Catch ex As Exception
            Me.metasyn.Remove(k)
            Me.metasyn.Add(o, k)
        End Try

        Try
            Me.metasyn.Add(k, o)
        Catch ex As Exception
            Me.metasyn.Remove(o)
            Me.metasyn.Add(k, o)
        End Try

    End Sub

    Public Function GetMetaSyn(ByVal k As String) As String
        Dim r As String
        Try
            k = k.Trim.ToUpper
            r = Me.metasyn.Item(k)
        Catch ex As Exception
            r = ""
        End Try
        Return r
    End Function

    Public Sub AddLinkTaxa(ByVal k As String, ByVal o As String)

        k = k.Trim.ToUpper
        o = o.Trim.ToUpper
        Try
            Me.linktaxa.Add(o, k)
        Catch ex As Exception
            Me.linktaxa.Remove(k)
            Me.linktaxa.Add(o, k)
        End Try

        Try
            Me.linktaxa.Add(k, o)
        Catch ex As Exception
            Me.linktaxa.Remove(o)
            Me.linktaxa.Add(k, o)
        End Try

    End Sub

    Public Sub AddCmt(ByVal k As String, ByVal o As String)

        Try
            Me.comment.Add(o, k)
        Catch ex As Exception
            Me.comment.Remove(k)
            Me.comment.Add(o, k)
        End Try

    End Sub

    Public Function GetCmt(ByVal k As String) As String
        Dim r As String
        Try
            r = Me.comment.Item(k)
        Catch ex As Exception
            r = ""
        End Try
        Return r
    End Function

    Public Function GetLinkTaxa(ByVal k As String) As String
        Dim r As String
        Try
            k = k.Trim.ToUpper
            r = Me.linktaxa.Item(k)
        Catch ex As Exception
            r = ""
        End Try
        Return r
    End Function

    Private Sub putLine(ByVal k As String, ByVal v As String, ByVal d As String)
        f.putline(k & d & v)
    End Sub

    Public Sub SaveSettings()
        Dim fn As String
        Dim fn2 As String
        fn = Path.Combine(Me.HomeDir, minifile)

        fn2 = Util.ChFileType(fn, "BAK")
        lf_rename(fn, fn2)
        f = New LFFile()
        f.openfileforwrite(fn)

        f.putline("# DIR")
        Me.putLine("TEXTDIR", Me.TextDir, "=")
        Me.putLine("DATADIR", Me.DataDir, "=")
        Me.putLine("DBDIR", Me.DBDir, "=")
        Me.putLine("MDBDIR", Me.DBDir, "=")
        Me.putLine("GAISVOL", Me.GaisVol, "=")

        f.putline("# FILE")
        Me.putLine("FLORAEXE", Me.FloraExe, "=")
        Me.putLine("GAISEXE", Me.GaisExe, "=")
        Me.putLine("OUTFILE", Me.OutFile, "=")
        Me.putLine("USERCMD", Me.UserCmd, "=")

        f.putline("# MAIN CFG")
        Me.putLine("CMDTPL", Me.CmdTpl, "=")

        f.putline("# START UP")
        Me.putLine("DELCACHE", Me.DeleteCache, "=")

        Dim s As String
        For Each s In mUnchangeableRest
            f.putline(s)
        Next
        f.closefile()
    End Sub
End Class
