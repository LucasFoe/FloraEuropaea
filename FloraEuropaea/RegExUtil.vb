Imports System.Text.RegularExpressions


Public Class RegExUtil
    ''' <summary>
    ''' Function matches with regular expression
    ''' </summary>
    ''' <param name="s">input string</param>
    ''' <param name="p">regexp pattern</param>
    ''' <param name="ignorecaseflag">true if case should be ignored, else false</param>
    ''' <param name="multilineflag"></param>
    ''' <returns>array of matches, first element = matched String of first iteration</returns>
    Public Shared Function Matches ( _
                                    ByVal s As String, ByVal p As String, _
                                    Optional ByVal ignorecaseflag As Boolean = True, _
                                    Optional ByVal multilineflag As Boolean = True _
                                    ) As String()
        Dim i As Long
        Dim k As Integer
        Dim l As Integer
        Dim r As Regex

        If ignorecaseflag And multilineflag Then
            r = New Regex (p, RegexOptions.IgnoreCase Or RegexOptions.Multiline)
        ElseIf ignorecaseflag Then
            r = New Regex (p, RegexOptions.IgnoreCase)
        ElseIf multilineflag Then
            r = New Regex (p, RegexOptions.Multiline)
        Else
            r = New Regex (p)
        End If

        Dim ms As MatchCollection = r.Matches (s)
        Dim m As Match
        Dim ret(0) As String
        Dim g As Group
        i = 0
        If ms.Count > 0 Then
            For k = 0 To ms.Count - 1
                m = ms.Item (k)
                If m.Success Then
                    If k = 0 Then
                        ret (i) = m.Groups (0).Value
                        i = i + 1
                    End If
                    For l = 1 To m.Groups.Count - 1
                        g = m.Groups (l)
                        ReDim Preserve ret(i)
                        ret (i) = g.Value
                        If ret (i) <> ret (i - 1) Then
                            i = i + 1
                        End If
                    Next
                End If
            Next
            If i > 0 Then
                Return ret
            Else
                Return Nothing
            End If
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' Function replace with regular expression
    ''' </summary>
    ''' <param name="s">input string</param>
    ''' <param name="p">regexp pattern</param>
    ''' <param name="r">replacement string</param>
    ''' <param name="ignorecaseflag">true if case should be ignored, else false</param>
    ''' <returns>string with p replaced by r</returns>
    Public Shared Function ReplaceRegExp ( _
                                          ByVal s As String, _
                                          ByVal p As String, _
                                          ByVal r As String, _
                                          Optional ByVal ignorecaseflag As Boolean = False _
                                          ) As String
        If ignorecaseflag Then
            ReplaceRegExp = Regex.Replace (s, p, r, RegexOptions.IgnoreCase)
        Else
            ReplaceRegExp = Regex.Replace (s, p, r)
        End If
    End Function

    ''' <summary>
    ''' Function match with regular expression
    ''' </summary>
    ''' <param name="s">input string</param>
    ''' <param name="p">regexp pattern</param>
    ''' <param name="ignorecaseflag">true if case should be ignored, else false</param>
    ''' <param name="multilineflag"></param>
    ''' <returns>true if match</returns>
    Public Shared Function Match ( _
                                  ByRef s As String, ByRef p As String, _
                                  Optional ByRef ignorecaseflag As Boolean = False, _
                                  Optional ByVal multilineflag As Boolean = True _
                                  ) As Boolean

        Dim r As Regex

        If ignorecaseflag And multilineflag Then
            r = New Regex (p, RegexOptions.IgnoreCase Or RegexOptions.Multiline)
        ElseIf ignorecaseflag Then
            r = New Regex (p, RegexOptions.IgnoreCase)
        ElseIf multilineflag Then
            r = New Regex (p, RegexOptions.Multiline)
        Else
            r = New Regex (p)
        End If

        Match = r.IsMatch (s)

    End Function

    ''' <summary>
    ''' Converts pattern with wildcards (*, ?) to a valid regular expression
    ''' </summary>
    Public Shared Function WildcardToRegex (ByVal pattern As String) As String
        Return Regex.Escape (pattern).Replace ("\*", ".*").Replace ("\?", ".")
    End Function

End Class

