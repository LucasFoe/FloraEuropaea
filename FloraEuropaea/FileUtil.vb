Imports System.IO
Module FileUtil

    ' rename file
    Public Sub lf_rename(ByVal fpath As String, ByVal fpathnew As String)

        If lf_fileExists(fpathnew) > -1 Then
            lf_delete(fpathnew)
        End If

        Rename(fpath, fpathnew)

    End Sub

    ' rename file
    Public Sub lf_rename2(ByVal wdir As String, ByVal fname As String, ByVal fnamenew As String)

        Dim wffrom, wfto As String
        wffrom = lf_filename(wdir, fname)
        wfto = lf_filename(wdir, fnamenew)
        lf_rename(wffrom, wfto)

    End Sub

    ' copy file
    Public Sub lf_copy(ByRef fpath As String, ByRef fpathnew As String)

        FileCopy(fpath, fpathnew)

    End Sub

    ' delete file
    Public Sub lf_delete(ByRef fpath As String)

        If lf_fileExists(fpath) >= 0 Then
            Kill(fpath)
        End If

    End Sub

    ' build filename out of dir and name
    Public Function lf_filename(ByRef wdir As String, ByRef fileName As String) As String

        Dim fpath As String
        fpath = wdir & "/" & fileName
        fpath = Replace(fpath, "\", "/")
        fpath = Replace(fpath, "//", "/")
        fpath = Replace(fpath, "//", "/")
        fpath = Replace(fpath, "//", "/")

        lf_filename = fpath

    End Function

    ' returns 1 if file exists
    Function lf_fileExists(ByRef fpath As String) As Short
        Dim lSize As Integer
        On Error Resume Next
        '* set lSize to -1
        lSize = -1
        ' 
        Dim fFile As New FileInfo(fpath)

        If Not fFile.Exists Then
            ' File does not exist
            lf_fileExists = -1
        Else
            lSize = fFile.Length
            If lSize = 0 Then
                ' File is zero bytes and exists
                lf_fileExists = 0
            ElseIf lSize > 0 Then
                ' File is not zero bytes and exists
                lf_fileExists = 1
            End If
        End If
    End Function

    ' returns 1 if file exists
    Function lf_fileExists2(ByVal wdir As String, ByVal fileName As String) As Short
        Dim fp As String
        fp = lf_filename(wdir, fileName)
        lf_fileExists2 = lf_fileExists(fp)
    End Function

    Function lf_getContent(ByRef fpath As String) As String
        Dim sr As New IO.StreamReader(fpath)
        lf_getContent = sr.ReadToEnd
        sr.Close()
    End Function

    Function lf_getContent2(ByVal wdir As String, ByVal fileName As String) As String
        Dim fp As String
        fp = lf_filename(wdir, fileName)
        lf_getContent2 = lf_getContent(fp)
    End Function

    Public Function lf_ListAllDrives() As String()
        Dim arDrives() As String
        arDrives = Directory.GetLogicalDrives()
        Return arDrives
    End Function

    Function lf_DriveInfo(ByVal d As String) As DriveInfo
        Dim dr As New DriveInfo(d)
        Return dr
    End Function

    ' Locate File in Directory p
    Function lf_FindFileName(ByVal p As String, ByVal f As String) As String
        Dim pf As String = ""
        Try
            Dim di As New DirectoryInfo(p)
            Dim wf As String = f.ToUpper
            Dim fi As New FileInfo(f)
            For Each fi In di.GetFiles
                If fi.Name.ToUpper = wf Then
                    pf = fi.FullName
                    Exit For
                End If
            Next
        Catch ex As Exception
        End Try
        Return pf
    End Function

    Function lf_FindFileInfo(ByVal p As String, ByVal f As String) As FileInfo
        Dim fiReturn As FileInfo = Nothing
        Try
            Dim di As New DirectoryInfo(p)
            Dim wf As String = f.ToUpper
            Dim fi As New FileInfo(f)
            For Each fi In di.GetFiles
                If fi.Name.ToUpper = wf Then
                    fiReturn = fi
                    Exit For
                End If
            Next
        Catch ex As Exception
        End Try
        Return fiReturn
    End Function

End Module
