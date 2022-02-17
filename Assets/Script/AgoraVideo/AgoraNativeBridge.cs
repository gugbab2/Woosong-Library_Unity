using System;
using System.Text;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;

//아고라 화면공유 정보 가져오기
public static class AgoraNativeBridge
{
    //화면공유 컨트롤 최대 인원
    const int DisplayCountMax = 10;
    //공유할 때 버퍼 사이즈
    const int CharBufferSize = 100000;
    //윈도우 정보
    private static Dictionary<string, IntPtr> windowInfo;
    //사용자 정보
    private static List<MonitorInfoWithHandle> displayInfo;
    //윈도우 정보를 받아오는 메서드
    public static MonitorInfoWithHandle[] GetWinDisplayInfo()
    {
        displayInfo = new List<MonitorInfoWithHandle>();
        EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, FilterDisplayCallback, IntPtr.Zero);
        return displayInfo.ToArray();
    }

    //윈도우 핸들이랑 리스트를 받아오는 메서드
    public static void GetDesktopWindowHandlesAndTitles(out Dictionary<string, IntPtr> info)
    {
        windowInfo = new Dictionary<string, IntPtr>();

        if (!EnumDesktopWindows(IntPtr.Zero, FilterCallback,
            IntPtr.Zero))
        {
            info = null;
        }
        else
        {
            info = windowInfo;
        }
    }
    //윈도우 필터 할 때 쓰는 메서드
    private static bool FilterCallback(IntPtr hWnd, int lParam)
    {
        // 타이틀 세팅
        StringBuilder sb_title = new StringBuilder(1024);
        GetWindowText(hWnd, sb_title, sb_title.Capacity);
        string title = sb_title.ToString();

        // 타이틀이 있을 경우 저장
        if (IsWindowVisible(hWnd) &&
            string.IsNullOrEmpty(title) == false)
        {
            if (windowInfo.ContainsKey(title)) title = string.Format("{0}{1}", title, hWnd);
            windowInfo.Add(title, hWnd);
        }
        return true;
    }
    //필터 콜백 메서드
    private static bool FilterDisplayCallback(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData)
    {
        var mi = new MONITORINFO();
        mi.size = (uint)Marshal.SizeOf(mi);
        GetMonitorInfo(hMonitor, ref mi);

        // Add to monitor info
        displayInfo.Add(new MonitorInfoWithHandle(hMonitor, mi));
        return true;
    }
    #region 유저Dll에다 정보를얻고 받아오는 세팅 
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll", EntryPoint = "GetWindowText",
        ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int GetWindowText(IntPtr hWnd,
        StringBuilder lpWindowText, int nMaxCount);

    [DllImport("user32.dll", EntryPoint = "EnumDesktopWindows",
        ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool EnumDesktopWindows(IntPtr hDesktop,
        EnumDelegate lpEnumCallbackFunction, IntPtr lParam);

    [DllImport("user32.dll", EntryPoint = "EnumDisplayMonitors",
        ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip,
        EnumMonitorsDelegate lpEnumCallbackFunction, IntPtr dwData);

    [DllImport("user32.dll", EntryPoint = "GetMonitorInfo",
        ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool GetMonitorInfo(IntPtr hmon, ref MONITORINFO mi);
    #endregion
    //콜백 딜리게이트 타입
    private delegate bool EnumDelegate(IntPtr hWnd, int lParam);

    private delegate bool EnumMonitorsDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData);

    #region  모니터 정보 핸들 인터페이스
    public interface IMonitorInfoWithHandle
    {
        IntPtr MonitorHandle { get; }
        MONITORINFO MonitorInfo { get; }
    }

    public class MonitorInfoWithHandle : IMonitorInfoWithHandle
    {
        public IntPtr MonitorHandle { get; private set; }
        public MONITORINFO MonitorInfo { get; private set; }

        public MonitorInfoWithHandle(IntPtr monitorHandle, MONITORINFO monitorInfo)
        {
            MonitorHandle = monitorHandle;
            MonitorInfo = monitorInfo;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }
    
    
    [StructLayout(LayoutKind.Sequential)]
    public struct MONITORINFO
    {
        public uint size;
        public RECT monitor;
        public RECT work;
        public uint flags;
    }
    #endregion 
}