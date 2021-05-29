Imports System.Drawing.Printing
Imports System.Runtime.InteropServices

Module Win32Api



    Public Const EM_LINEFROMCHAR As Long = &HC9
    Public Const EM_LINEINDEX As Long = &HBB
    Public Const EM_GETLINECOUNT As Long = &HBA
    Public Const EM_LINESCROLL As Long = &HB6

    Public Const WM_VSCROLL As Long = &H115
    Public Const WM_HSCROLL As Long = &H114
    Public Const SB_LINEDOWN As Long = 1
    Public Const SB_LINEUP As Long = 0
    Public Const SB_TOP As Long = 6
    Public Const SB_BOTTOM As Long = 7
    Public Const SB_PAGEUP As Long = 2
    Public Const SB_PAGEDOWN As Long = 3

    ' Printing
    Public Const AnInch As Double = 14.4
    Public Const WM_USER As Integer = &H400
    Public Const EM_FORMATRANGE As Integer = WM_USER + 57

    '*******************
    ' ScrollToLineOfFirstMatch
    ' scroll to lineIndexofFirstMatch
    '*******************
    Public Sub ScrollToLineOfFirstMatch(ByVal tBox As RichTextBox, ByVal lFirstMatchPosition As Integer)
        Dim i As Long, li As Integer
        Top(tBox)
        li = tBox.GetLineFromCharIndex(lFirstMatchPosition)
        For i = 1 To li
            LineDown(tBox)
        Next
    End Sub

    '*******************
    ' LineUp
    ' scroll 1 line up
    '*******************
    Public Sub LineUp(ByVal tBox As RichTextBox)
        NativeMethods.SendMessage(tBox.Handle, WM_VSCROLL, SB_LINEUP, 0)
    End Sub

    '*******************
    ' LineDown
    ' scroll 1 line down
    '*******************
    Public Sub LineDown(ByVal tBox As RichTextBox)
        NativeMethods.SendMessage(tBox.Handle, WM_VSCROLL, SB_LINEDOWN, 0)
    End Sub

    '*******************
    ' PageUp
    ' scroll 1 page up
    '*******************
    Public Sub PageUp(ByVal tBox As RichTextBox)
        NativeMethods.SendMessage(tBox.Handle, WM_VSCROLL, SB_PAGEUP, 0)
    End Sub

    '*******************
    ' PageDown
    ' scroll 1 page down
    '*******************
    Public Sub PageDown(ByVal tBox As RichTextBox)
        NativeMethods.SendMessage(tBox.Handle, WM_VSCROLL, SB_PAGEDOWN, 0)
    End Sub

    '*******************
    ' Top
    ' scroll to top of page
    '*******************
    Public Sub Top(ByVal tBox As RichTextBox)
        NativeMethods.SendMessage(tBox.Handle, WM_VSCROLL, SB_TOP, 0)
    End Sub

    '*******************
    ' Top
    ' scroll to bottom of page
    '*******************
    Public Sub Bottom(ByVal tBox As RichTextBox)
        NativeMethods.SendMessage(tBox.Handle, WM_VSCROLL, SB_BOTTOM, 0)
    End Sub

    '*******************
    ' GetLineCount
    ' Get the line count
    '*******************
    Public Function GetLineCount(ByVal tBox As RichTextBox) As Long
        GetLineCount = NativeMethods.SendMessage(tBox.Handle, EM_GETLINECOUNT, 0&, 0&)
    End Function

    '*******************
    ' GetLineNum
    ' Get current line number
    '*******************
    Public Function GetLineNum(ByVal tBox As RichTextBox) As Long
        GetLineNum = NativeMethods.SendMessage(tBox.Handle, EM_LINEFROMCHAR, tBox.SelectionStart, 0&)
    End Function

    '*******************
    ' AdjustTextDisplay
    ' place current position
    ' at top of display, and
    ' scroll display up 2 lines
    '*******************
    Public Sub AdjustTextDisplay(ByVal tBox As RichTextBox)
        Dim cPos As Long
        Dim cLen As Long

        With tBox
            cPos = .SelectionStart       'Save selection
            cLen = .SelectionLength      'Save anything highlighted ' ACE="Courier New" SIZE="2" COLOR="#00008 ' 0">
            .SelectionStart = Len(.Text) 'bottom of text
            .SelectionStart = cPos       'force top of display
            .SelectionLength = cLen      'reselecting any selection
            Call NativeMethods.SendMessage(tBox.Handle, EM_LINESCROLL, 0&, -2&)
        End With
    End Sub

    <StructLayout(LayoutKind.Sequential)>
    Private Structure RECT
        Public Left As Integer
        Public Top As Integer
        Public Right As Integer
        Public Bottom As Integer
    End Structure

    Private Structure CHARRANGE
        Public cpMin As Integer          ' First character of range (0 for start of doc)
        Public cpMax As Integer          ' Last character of range (-1 for end of doc)
    End Structure

    Private Structure FORMATRANGE
        Public hdc As IntPtr             ' Actual DC to draw on
        Public hdcTarget As IntPtr       ' Target DC for determining text formatting
        Public rc As RECT                ' Region of the DC to draw to (in twips)
        Public rcPage As RECT            ' Region of the whole DC (page size) (in twips)
        Public chrg As CHARRANGE         ' Range of text to draw (see above declaration)
    End Structure

    ' Help: Knowledge Base 811401
    ' Render the contents of the RichTextBox for printing
    ' Return the last character printed + 1 (printing start from this point for next page)
    Public Function Print(ByVal rtb As RichTextBox, ByVal charFrom As Integer, ByVal charTo As Integer, ByVal e As PrintPageEventArgs) As Integer

        ' Mark starting and ending character 
        Dim cRange As CHARRANGE
        cRange.cpMin = charFrom
        cRange.cpMax = charTo

        ' Calculate the area to render and print
        Dim rectToPrint As RECT
        rectToPrint.Top = e.MarginBounds.Top * AnInch
        rectToPrint.Bottom = e.MarginBounds.Bottom * AnInch
        rectToPrint.Left = e.MarginBounds.Left * AnInch
        rectToPrint.Right = e.MarginBounds.Right * AnInch

        ' Calculate the size of the page
        Dim rectPage As RECT
        rectPage.Top = e.PageBounds.Top * AnInch
        rectPage.Bottom = e.PageBounds.Bottom * AnInch
        rectPage.Left = e.PageBounds.Left * AnInch
        rectPage.Right = e.PageBounds.Right * AnInch

        Dim hdc As IntPtr = e.Graphics.GetHdc()

        Dim fmtRange As FORMATRANGE
        fmtRange.chrg = cRange                 ' Indicate character from to character to 
        fmtRange.hdc = hdc                     ' Use the same DC for measuring and rendering
        fmtRange.hdcTarget = hdc               ' Point at printer hDC
        fmtRange.rc = rectToPrint              ' Indicate the area on page to print
        fmtRange.rcPage = rectPage             ' Indicate whole size of page

        Dim res As IntPtr = IntPtr.Zero

        Dim wparam As IntPtr = IntPtr.Zero
        wparam = New IntPtr(1)

        ' Move the pointer to the FORMATRANGE structure in memory
        Dim lparam As IntPtr = IntPtr.Zero
        lparam = Marshal.AllocCoTaskMem(Marshal.SizeOf(fmtRange))
        Marshal.StructureToPtr(fmtRange, lparam, False)

        ' Send the rendered data for printing 
        res = NativeMethods.SendMessage(rtb.Handle, EM_FORMATRANGE, wparam, lparam)

        ' Free the block of memory allocated
        Marshal.FreeCoTaskMem(lparam)

        ' Release the device context handle obtained by a previous call
        e.Graphics.ReleaseHdc(hdc)

        ' Return last + 1 character printer
        Return res.ToInt32()
    End Function

End Module
