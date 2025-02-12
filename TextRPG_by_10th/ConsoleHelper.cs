using System.Runtime.InteropServices;

namespace TextRPG_by_10th
{
    public class ConsoleHelper
    {
        // 고정폭 트루타입 글꼴 상수
        private const int FixedWidthTrueType = 54;
        // 표준 출력 핸들 상수
        private const int StandardOutputHandle = -11;

        // 표준 출력 핸들을 가져오는 외부 메서드
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        // 콘솔 글꼴 설정을 변경하는 외부 메서드
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern bool SetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool MaximumWindow, ref FontInfo ConsoleCurrentFontEx);

        // 현재 콘솔 글꼴 설정을 가져오는 외부 메서드
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern bool GetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool MaximumWindow, ref FontInfo ConsoleCurrentFontEx);

        // 콘솔 화면 버퍼 크기를 설정하는 외부 메서드
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetConsoleScreenBufferSize(IntPtr hConsoleOutput, COORD size);

        // COORD 구조체 정의
        [StructLayout(LayoutKind.Sequential)]
        public struct COORD
        {
            public short X;
            public short Y;
        }

        // ConsoleOutputHandle을 저장하는 변수
        private static readonly IntPtr ConsoleOutputHandle = GetStdHandle(StandardOutputHandle);

        // 글꼴 정보를 저장하는 구조체
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct FontInfo
        {
            internal int cbSize;
            internal int FontIndex;
            internal short FontWidth;
            public short FontSize;
            public int FontFamily;
            public int FontWeight;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string FontName;
        }

        // 현재 콘솔 글꼴 정보를 가져오는 메서드
        public static FontInfo GetCurrentFont()
        {
            FontInfo before = new FontInfo
            {
                cbSize = Marshal.SizeOf<FontInfo>()
            };

            // GetCurrentConsoleFontEx 메서드 호출
            if (!GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref before))
            {
                var er = Marshal.GetLastWin32Error();
                throw new System.ComponentModel.Win32Exception(er);
            }
            return before;
        }

        // 콘솔 글꼴을 변경하고 변경 전, 후의 정보를 반환하는 메서드
        public static FontInfo[] SetCurrentFont(string font, short fontSize = 0)
        {
            FontInfo before = new FontInfo
            {
                cbSize = Marshal.SizeOf<FontInfo>()
            };

            // GetCurrentConsoleFontEx 메서드 호출
            if (GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref before))
            {
                FontInfo set = new FontInfo
                {
                    cbSize = Marshal.SizeOf<FontInfo>(),
                    FontIndex = 0,
                    FontFamily = FixedWidthTrueType,
                    FontName = font,
                    FontWeight = 400,
                    FontSize = fontSize > 0 ? fontSize : before.FontSize
                };

                // SetCurrentConsoleFontEx 메서드 호출
                if (!SetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref set))
                {
                    var ex = Marshal.GetLastWin32Error();
                    throw new System.ComponentModel.Win32Exception(ex);
                }

                // Reset console buffer size and window size
                SetConsoleScreenBufferSize(ConsoleOutputHandle, new COORD { X = 80, Y = 300 });

                FontInfo after = new FontInfo
                {
                    cbSize = Marshal.SizeOf<FontInfo>()
                };
                GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref after);

                return new[] { before, set, after };
            }
            else
            {
                var er = Marshal.GetLastWin32Error();
                throw new System.ComponentModel.Win32Exception(er);
            }
        }
    }
}
