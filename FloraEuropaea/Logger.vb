
Imports System.IO
Imports System
Imports System.Reflection


Public Enum LogType
    Info = 1
    Warning = 2
    xError = 3
End Enum

' Class for handling of log messages
Public Class Logger

    ' Privates
    Private isOpen As Boolean = False
    Private swLog As StreamWriter
    Private strLogFile As String
    Private keepOpenFlag As Boolean

    ' Constructors
    Public Sub New(Optional ByVal ko As Boolean = False)
        Dim logpath As String = GetApplicationFolderName()
        Me.strLogFile = Path.Combine(logpath, GetNewLogFilename())
        Me.keepOpenFlag = ko
    End Sub

    Public Sub ForceCloseFile()
        keepOpenFlag = False
        closeFile()
    End Sub

    ' Verzeichnis von aktueller Application ermitteln
    Private Function GetApplicationFolderName() As String
        ' FileInfo-Objekt für die Datei erzeugen, die die Eintritts-Assembly speichert
        Dim fi As FileInfo = New FileInfo(Assembly.GetEntryAssembly().Location)
        ' Den Pfad des Ordners der Datei zurckgeben
        Return fi.DirectoryName
    End Function

    Protected Overrides Sub Finalize()
        Try
            ForceCloseFile()
        Finally
            MyBase.Finalize()
        End Try
    End Sub

    Private Sub openFile()
        If isOpen Then
            Return
        End If
        Try
            swLog = File.AppendText(strLogFile)
            isOpen = True
        Catch
            isOpen = False
        End Try
    End Sub

    Private Sub closeFile()
        If isOpen Then
            swLog.Close()
            Me.isOpen = False
        End If
    End Sub

    ' Build name of log file out of date and time
    Private Shared Function GetNewLogFilename() As String
        Return DateTime.Now.ToString("dd-MM-yyyy") + ".log"
    End Function

    ' Write log line
    Public Sub WriteLine(ByVal logtype As LogType, ByVal message As String)
        Dim stub As String = DateTime.Now.ToString("dd-MM-yyyy @ HH:MM:ss ")
        Select Case logtype
            Case logtype.Info
                stub += "Informational , "
                Exit Select
            Case logtype.Warning
                stub += "Warning , "
                Exit Select
            Case logtype.xError
                stub += "Fatal error , "
                Exit Select
        End Select
        stub += message
        writelog(stub)
    End Sub

    Public Sub WriteLog(ByVal msg As String)
        openFile()
        If isOpen Then
            swLog.WriteLine(msg)
        Else
            Console.WriteLine("Error Cannot write to log file.")
        End If
        closeFile()
    End Sub

    Public Sub WriteLog(ByVal msglist As List(Of String))
        Dim msg As String
        For Each msg In msglist
            WriteLog(msg)
        Next
    End Sub

End Class

