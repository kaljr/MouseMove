using System;
using System.Runtime.InteropServices;

namespace MoveMouse
{
    public class InputControl
    {
        // This is the current recommended method of updating user input queue, including mouse events
        // It will inject some input events into the input queue to be processed as normal input
        [DllImport("user32.dll")]
        private static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray), In] LPINPUT[] pInputs, int cbSize);

        // This function sets the absolute coords of the point, but doesn't work in
        // some program windows (good for general Windows use though)
        [DllImport("User32.Dll")]
        public static extern long SetCursorPos(int x, int y);

        // This method, which only moves the mouse by a delta, works inside of all programs, but has been deprecated
        // Can use (int)MOUSEEVENTF.ABSOLUTE | (int)MOUSEEVENTF.MOVE, but then has same limitations as SetCursorPos
        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        [DllImport("User32.Dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref POINT point);

        [DllImport("user32.dll", SetLastError = false)]
        private static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll", SetLastError = false)]
        private static extern IntPtr GetForegroundWindow();


        [DllImport("user32.dll")]
        private static extern IntPtr GetMessageExtraInfo();

       
        public static void Move(int xDelta, int yDelta)
        {
            mouse_event((int)MOUSEEVENTF.MOVE, xDelta, yDelta, 0, 0);
        }

        public static void SendInputArray(LPINPUT[] inputs)
        {
            uint sentInputs = InputControl.SendInput((uint)inputs.Length, inputs, LPINPUT.Size);
            Console.WriteLine("Successfully sent {0} Inputs", sentInputs);
        }

        public static LPINPUT CreateMouseMoveInput(int x, int y)
        {
            return new LPINPUT
            {
                type = (int)InputType.MOUSE,
                Data = new InputUnion
                {
                    mi = new MOUSEINPUT
                    {
                        dx = x,
                        dy = y,
                        dwFlags = MOUSEEVENTF.MOVE,
                        dwExtraInfo = UIntPtr.Zero
                    }
                }
            };
        }

        public static LPINPUT CreateKeyboardScancodeInput(short scancode, KEYEVENTF flags)
        {
            return new LPINPUT
            {
                type = (int)InputType.KEYBOARD,
                Data = new InputUnion
                {
                    ki = new KEYBDINPUT
                    {
                        wVk = 0,
                        wScan = scancode,
                        dwFlags = flags | KEYEVENTF.SCANCODE,
                        dwExtraInfo = UIntPtr.Zero
                    }
                }
            };
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int x;
        public int y;

        public POINT(int X, int Y)
        {
            x = X;
            y = Y;
        }
    }


    // Below is mostly the code to set up the data structure requirements for SendInput
    [StructLayout(LayoutKind.Sequential)]
    public struct LPINPUT
    {
        public int type;
        public InputUnion Data;
        public static int Size { get { return Marshal.SizeOf(typeof(LPINPUT)); } }
    }

    // Union structure
    [StructLayout(LayoutKind.Explicit)]
    public struct InputUnion
    {
        [FieldOffset(0)]
        public MOUSEINPUT mi;
        [FieldOffset(0)]
        public KEYBDINPUT ki;
        [FieldOffset(0)]
        public HARDWAREINPUT hi;
    }

    public enum InputType
    {
        MOUSE = 0,
        KEYBOARD = 1,
        HARDWARE = 2
    }

    // Input Types
    [StructLayout(LayoutKind.Sequential)]
    public struct MOUSEINPUT
    {
        public int dx;
        public int dy;
        public int mouseData;
        public MOUSEEVENTF dwFlags;
        public uint time;
        public UIntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct KEYBDINPUT
    {
        public short wVk;
        public short wScan;
        public KEYEVENTF dwFlags;
        public int time;
        public UIntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HARDWAREINPUT
    {
        public int uMsg;
        public short wParamL;
        public short wParamH;
    }

    [Flags]
    public enum KEYEVENTF : uint
    {
        KEYDOWN = 0x0,
        EXTENDEDKEY = 0x0001,
        KEYUP = 0x0002,
        SCANCODE = 0x0008,
        UNICODE = 0x0004
    }

    [Flags]
    public enum MOUSEEVENTF : uint
    {
        ABSOLUTE = 0x8000,
        HWHEEL = 0x01000,
        MOVE = 0x0001,
        MOVE_NOCOALESCE = 0x2000,
        LEFTDOWN = 0x0002,
        LEFTUP = 0x0004,
        RIGHTDOWN = 0x0008,
        RIGHTUP = 0x0010,
        MIDDLEDOWN = 0x0020,
        MIDDLEUP = 0x0040,
        VIRTUALDESK = 0x4000,
        WHEEL = 0x0800,
        XDOWN = 0x0080,
        XUP = 0x0100
    }
}
