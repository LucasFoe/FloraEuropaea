Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Drawing.Printing

Friend NotInheritable Class NativeMethods

    ' Help
    Const SW_NORMAL As Long = 1


    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Friend Shared Function SendMessage(
                                                                    ByVal hwnd As IntPtr,
                                                                    ByVal wMsg As UInt32,
                                                                    ByVal wParam As IntPtr,
                                                                    ByVal lParam As IntPtr
                                                                    ) As IntPtr
    End Function



End Class
