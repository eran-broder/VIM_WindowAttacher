using System.Runtime.InteropServices;
using System.Text;

namespace Vigor.Windows.Native;

public static class Native
{
    [Flags]
    public enum WindowStylesEx : uint
    {
        WS_EX_ACCEPTFILES = 0x00000010,
        WS_EX_APPWINDOW = 0x00040000,
        WS_EX_CLIENTEDGE = 0x00000200,
        WS_EX_COMPOSITED = 0x02000000,
        WS_EX_CONTEXTHELP = 0x00000400,
        WS_EX_CONTROLPARENT = 0x00010000,
        WS_EX_DLGMODALFRAME = 0x00000001,
        WS_EX_LAYERED = 0x00080000,
        WS_EX_LAYOUTRTL = 0x00400000,
        WS_EX_LEFT = 0x00000000,
        WS_EX_LEFTSCROLLBAR = 0x00004000,
        WS_EX_LTRREADING = 0x00000000,
        WS_EX_MDICHILD = 0x00000040,
        WS_EX_NOACTIVATE = 0x08000000,
        WS_EX_NOINHERITLAYOUT = 0x00100000,
        WS_EX_NOPARENTNOTIFY = 0x00000004,
        WS_EX_NOREDIRECTIONBITMAP = 0x00200000,
        WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE,
        WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST,
        WS_EX_RIGHT = 0x00001000,
        WS_EX_RIGHTSCROLLBAR = 0x00000000,
        WS_EX_RTLREADING = 0x00002000,
        WS_EX_STATICEDGE = 0x00020000,
        WS_EX_TOOLWINDOW = 0x00000080,
        WS_EX_TOPMOST = 0x00000008,
        WS_EX_TRANSPARENT = 0x00000020,
        WS_EX_WINDOWEDGE = 0x00000100
    }

    [Flags]
    public enum WindowStyles : uint
    {
        WS_BORDER = 0x800000,
        WS_CAPTION = 0xc00000,
        WS_CHILD = 0x40000000,
        WS_CLIPCHILDREN = 0x2000000,
        WS_CLIPSIBLINGS = 0x4000000,
        WS_DISABLED = 0x8000000,
        WS_DLGFRAME = 0x400000,
        WS_GROUP = 0x20000,
        WS_HSCROLL = 0x100000,
        WS_MAXIMIZE = 0x1000000,
        WS_MAXIMIZEBOX = 0x10000,
        WS_MINIMIZE = 0x20000000,
        WS_MINIMIZEBOX = 0x20000,
        WS_OVERLAPPED = 0x0,
        WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_SIZEFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
        WS_POPUP = 0x80000000u,
        WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
        WS_SIZEFRAME = 0x40000,
        WS_SYSMENU = 0x80000,
        WS_TABSTOP = 0x10000,
        WS_VISIBLE = 0x10000000,
        WS_VSCROLL = 0x200000
    }
    public enum WindowLongFlags : int
    {
        GWL_EXSTYLE = -20,
        GWLP_HINSTANCE = -6,
        GWLP_HWNDPARENT = -8,
        GWL_ID = -12,
        GWL_STYLE = -16,
        GWL_USERDATA = -21,
        GWL_WNDPROC = -4,
        DWLP_USER = 0x8,
        DWLP_MSGRESULT = 0x0,
        DWLP_DLGPROC = 0x4
    }

    [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
    public static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

    // This static method is required because legacy OSes do not support
    // SetWindowLongPtr
    public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
    {
        if (IntPtr.Size == 8)
            return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
        else
            return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
    }

    [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
    private static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
    private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetCursorPos(out POINT lpPoint);

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;

        public POINT(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr SetWindowsHookEx(HookType hookType, HookProc lpfn, IntPtr hMod, uint dwThreadId);

    public delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam,
        IntPtr lParam);

    // overload for use with LowLevelKeyboardProc
    [DllImport("user32.dll")]
    public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, WM wParam, [In] KBDLLHOOKSTRUCT lParam);

    // overload for use with LowLevelMouseProc
    [DllImport("user32.dll")]
    public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, WM wParam, [In] MSLLHOOKSTRUCT lParam);

    [StructLayout(LayoutKind.Sequential)]
    public class KBDLLHOOKSTRUCT
    {
        public uint vkCode;
        public uint scanCode;
        public KBDLLHOOKSTRUCTFlags flags;
        public uint time;
        public UIntPtr dwExtraInfo;
    }
    [Flags]
    public enum KBDLLHOOKSTRUCTFlags : uint
    {
        LLKHF_EXTENDED = 0x01,
        LLKHF_INJECTED = 0x10,
        LLKHF_ALTDOWN = 0x20,
        LLKHF_UP = 0x80,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MSLLHOOKSTRUCT
    {
        public POINT pt;
        public int mouseData; // be careful, this must be ints, not uints (was wrong before I changed it...). regards, cmew.
        public int flags;
        public int time;
        public UIntPtr dwExtraInfo;
    }
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
    
    public delegate bool EnumWindowsProc(IntPtr hWnd, ref IntPtr lParam);

    [DllImport("user32.dll")]
    public static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn,
        IntPtr lParam);

    public delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

    [DllImport("user32.dll")]
    public static extern IntPtr GetWindowRect(IntPtr hWnd, out RECT rect);

    [DllImport("user32.dll")]
    public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public int Height => bottom - top;
        public int Width => right - left;
    }

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool BringWindowToTop(IntPtr hWnd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr GetWindow(IntPtr hWnd, GetWindowType uCmd);

    public enum GetWindowType : uint
    {
        /// <summary>
        /// The retrieved handle identifies the window of the same type that is highest in the Z order.
        /// <para/>
        /// If the specified window is a topmost window, the handle identifies a topmost window.
        /// If the specified window is a top-level window, the handle identifies a top-level window.
        /// If the specified window is a child window, the handle identifies a sibling window.
        /// </summary>
        GW_HWNDFIRST = 0,
        /// <summary>
        /// The retrieved handle identifies the window of the same type that is lowest in the Z order.
        /// <para />
        /// If the specified window is a topmost window, the handle identifies a topmost window.
        /// If the specified window is a top-level window, the handle identifies a top-level window.
        /// If the specified window is a child window, the handle identifies a sibling window.
        /// </summary>
        GW_HWNDLAST = 1,
        /// <summary>
        /// The retrieved handle identifies the window below the specified window in the Z order.
        /// <para />
        /// If the specified window is a topmost window, the handle identifies a topmost window.
        /// If the specified window is a top-level window, the handle identifies a top-level window.
        /// If the specified window is a child window, the handle identifies a sibling window.
        /// </summary>
        GW_HWNDNEXT = 2,
        /// <summary>
        /// The retrieved handle identifies the window above the specified window in the Z order.
        /// <para />
        /// If the specified window is a topmost window, the handle identifies a topmost window.
        /// If the specified window is a top-level window, the handle identifies a top-level window.
        /// If the specified window is a child window, the handle identifies a sibling window.
        /// </summary>
        GW_HWNDPREV = 3,
        /// <summary>
        /// The retrieved handle identifies the specified window's owner window, if any.
        /// </summary>
        GW_OWNER = 4,
        /// <summary>
        /// The retrieved handle identifies the child window at the top of the Z order,
        /// if the specified window is a parent window; otherwise, the retrieved handle is NULL.
        /// The function examines only child windows of the specified window. It does not examine descendant windows.
        /// </summary>
        GW_CHILD = 5,
        /// <summary>
        /// The retrieved handle identifies the enabled popup window owned by the specified window (the
        /// search uses the first such window found using GW_HWNDNEXT); otherwise, if there are no enabled
        /// popup windows, the retrieved handle is that of the specified window.
        /// </summary>
        GW_ENABLEDPOPUP = 6
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern int GetWindowTextLength(IntPtr hWnd);

    public enum WinEvents : uint
    {
        /** The range of WinEvent constant values specified by the Accessibility Interoperability Alliance (AIA) for use across the industry.
    * For more information, see Allocation of WinEvent IDs. */
        EVENT_AIA_START = 0xA000,
        EVENT_AIA_END = 0xAFFF,

        /** The lowest and highest possible event values.
*/
        EVENT_MIN = 0x00000001,
        EVENT_MAX = 0x7FFFFFFF,

        /** An object's KeyboardShortcut property has changed. Server applications send this event for their accessible objects.
*/
        EVENT_OBJECT_ACCELERATORCHANGE = 0x8012,

        /** Sent when a window is cloaked. A cloaked window still exists, but is invisible to the user.
*/
        EVENT_OBJECT_CLOAKED = 0x8017,

        /** A window object's scrolling has ended. Unlike EVENT_SYSTEM_SCROLLEND, this event is associated with the scrolling window.
    * Whether the scrolling is horizontal or vertical scrolling, this event should be sent whenever the scroll action is completed. * The hwnd parameter of the WinEventProc callback function describes the scrolling window; the idObject parameter is OBJID_CLIENT, * and the idChild parameter is CHILDID_SELF. */
        EVENT_OBJECT_CONTENTSCROLLED = 0x8015,

        /** An object has been created. The system sends this event for the following user interface elements: caret, header control,
    * list-view control, tab control, toolbar control, tree view control, and window object. Server applications send this event * for their accessible objects. * Before sending the event for the parent object, servers must send it for all of an object's child objects. * Servers must ensure that all child objects are fully created and ready to accept IAccessible calls from clients before * the parent object sends this event. * Because a parent object is created after its child objects, clients must make sure that an object's parent has been created * before calling IAccessible::get_accParent, particularly if in-context hook functions are used. */
        EVENT_OBJECT_CREATE = 0x8000,

        /** An object's DefaultAction property has changed. The system sends this event for dialog boxes. Server applications send
    * this event for their accessible objects. */
        EVENT_OBJECT_DEFACTIONCHANGE = 0x8011,

        /** An object's Description property has changed. Server applications send this event for their accessible objects.
*/
        EVENT_OBJECT_DESCRIPTIONCHANGE = 0x800D,

        /** An object has been destroyed. The system sends this event for the following user interface elements: caret, header control,
    * list-view control, tab control, toolbar control, tree view control, and window object. Server applications send this event for * their accessible objects. * Clients assume that all of an object's children are destroyed when the parent object sends this event. * After receiving this event, clients do not call an object's IAccessible properties or methods. However, the interface pointer * must remain valid as long as there is a reference count on it (due to COM rules), but the UI element may no longer be present. * Further calls on the interface pointer may return failure errors; to prevent this, servers create proxy objects and monitor * their life spans. */
        EVENT_OBJECT_DESTROY = 0x8001,

        /** The user started to drag an element. The hwnd, idObject, and idChild parameters of the WinEventProc callback function
    * identify the object being dragged. */
        EVENT_OBJECT_DRAGSTART = 0x8021,

        /** The user has ended a drag operation before dropping the dragged element on a drop target. The hwnd, idObject, and idChild
    * parameters of the WinEventProc callback function identify the object being dragged. */
        EVENT_OBJECT_DRAGCANCEL = 0x8022,

        /** The user dropped an element on a drop target. The hwnd, idObject, and idChild parameters of the WinEventProc callback
    * function identify the object being dragged. */
        EVENT_OBJECT_DRAGCOMPLETE = 0x8023,

        /** The user dragged an element into a drop target's boundary. The hwnd, idObject, and idChild parameters of the WinEventProc
    * callback function identify the drop target. */
        EVENT_OBJECT_DRAGENTER = 0x8024,

        /** The user dragged an element out of a drop target's boundary. The hwnd, idObject, and idChild parameters of the WinEventProc
    * callback function identify the drop target. */
        EVENT_OBJECT_DRAGLEAVE = 0x8025,

        /** The user dropped an element on a drop target. The hwnd, idObject, and idChild parameters of the WinEventProc callback
    * function identify the drop target. */
        EVENT_OBJECT_DRAGDROPPED = 0x8026,

        /** The highest object event value.
*/
        EVENT_OBJECT_END = 0x80FF,

        /** An object has received the keyboard focus. The system sends this event for the following user interface elements:
    * list-view control, menu bar, pop-up menu, switch window, tab control, tree view control, and window object. * Server applications send this event for their accessible objects. * The hwnd parameter of the WinEventProc callback function identifies the window that receives the keyboard focus. */
        EVENT_OBJECT_FOCUS = 0x8005,

        /** An object's Help property has changed. Server applications send this event for their accessible objects.
*/
        EVENT_OBJECT_HELPCHANGE = 0x8010,

        /** An object is hidden. The system sends this event for the following user interface elements: caret and cursor.
    * Server applications send this event for their accessible objects. * When this event is generated for a parent object, all child objects are already hidden. * Server applications do not send this event for the child objects. * Hidden objects include the STATE_SYSTEM_INVISIBLE flag; shown objects do not include this flag. The EVENT_OBJECT_HIDE event * also indicates that the STATE_SYSTEM_INVISIBLE flag is set. Therefore, servers do not send the EVENT_STATE_CHANGE event in * this case. */
        EVENT_OBJECT_HIDE = 0x8003,

        /** A window that hosts other accessible objects has changed the hosted objects. A client might need to query the host
    * window to discover the new hosted objects, especially if the client has been monitoring events from the window. * A hosted object is an object from an accessibility framework (MSAA or UI Automation) that is different from that of the host. * Changes in hosted objects that are from the same framework as the host should be handed with the structural change events, * such as EVENT_OBJECT_CREATE for MSAA. For more info see comments within winuser.h. */
        EVENT_OBJECT_HOSTEDOBJECTSINVALIDATED = 0x8020,

        /** An IME window has become hidden.
*/
        EVENT_OBJECT_IME_HIDE = 0x8028,

        /** An IME window has become visible.
*/
        EVENT_OBJECT_IME_SHOW = 0x8027,

        /** The size or position of an IME window has changed.
*/
        EVENT_OBJECT_IME_CHANGE = 0x8029,

        /** An object has been invoked; for example, the user has clicked a button. This event is supported by common controls and is
    * used by UI Automation. * For this event, the hwnd, ID, and idChild parameters of the WinEventProc callback function identify the item that is invoked. */
        EVENT_OBJECT_INVOKED = 0x8013,

        /** An object that is part of a live region has changed. A live region is an area of an application that changes frequently
    * and/or asynchronously. */
        EVENT_OBJECT_LIVEREGIONCHANGED = 0x8019,

        /** An object has changed location, shape, or size. The system sends this event for the following user interface elements:
    * caret and window objects. Server applications send this event for their accessible objects. * This event is generated in response to a change in the top-level object within the object hierarchy; it is not generated for any * children that the object might have. For example, if the user resizes a window, the system sends this notification for the window, * but not for the menu bar, title bar, scroll bar, or other objects that have also changed. * The system does not send this event for every non-floating child window when the parent moves. However, if an application explicitly * resizes child windows as a result of resizing the parent window, the system sends multiple events for the resized children. * If an object's State property is set to STATE_SYSTEM_FLOATING, the server sends EVENT_OBJECT_LOCATIONCHANGE whenever the object changes * location. If an object does not have this state, servers only trigger this event when the object moves in relation to its parent. * For this event notification, the idChild parameter of the WinEventProc callback function identifies the child object that has changed. */
        EVENT_OBJECT_LOCATIONCHANGE = 0x800B,

        /** An object's Name property has changed. The system sends this event for the following user interface elements: check box,
    * cursor, list-view control, push button, radio button, status bar control, tree view control, and window object. Server * * applications send this event for their accessible objects. */
        EVENT_OBJECT_NAMECHANGE = 0x800C,

        /** An object has a new parent object. Server applications send this event for their accessible objects.
*/
        EVENT_OBJECT_PARENTCHANGE = 0x800F,

        /** A container object has added, removed, or reordered its children. The system sends this event for the following user
    * interface elements: header control, list-view control, toolbar control, and window object. Server applications send this * event as appropriate for their accessible objects. * For example, this event is generated by a list-view object when the number of child elements or the order of the elements changes. * This event is also sent by a parent window when the Z-order for the child windows changes. */
        EVENT_OBJECT_REORDER = 0x8004,

        /** The selection within a container object has changed. The system sends this event for the following user interface elements:
    * list-view control, tab control, tree view control, and window object. Server applications send this event for their accessible * objects. * This event signals a single selection: either a child is selected in a container that previously did not contain any selected children, * or the selection has changed from one child to another. * The hwnd and idObject parameters of the WinEventProc callback function describe the container; the idChild parameter identifies the object * that is selected. If the selected child is a window that also contains objects, the idChild parameter is OBJID_WINDOW. */
        EVENT_OBJECT_SELECTION = 0x8006,

        /** A child within a container object has been added to an existing selection. The system sends this event for the following user
    * interface elements: list box, list-view control, and tree view control. Server applications send this event for their accessible * objects. * The hwnd and idObject parameters of the WinEventProc callback function describe the container. The idChild parameter is the child that * is added to the selection. */
        EVENT_OBJECT_SELECTIONADD = 0x8007,

        /** An item within a container object has been removed from the selection. The system sends this event for the following user
    * interface elements: list box, list-view control, and tree view control. Server applications send this event for their accessible * objects. * This event signals that a child is removed from an existing selection. * The hwnd and idObject parameters of the WinEventProc callback function describe the container; the idChild parameter identifies * the child that has been removed from the selection. */
        EVENT_OBJECT_SELECTIONREMOVE = 0x8008,

        /** Numerous selection changes have occurred within a container object. The system sends this event for list boxes; server
    * applications send it for their accessible objects. * This event is sent when the selected items within a control have changed substantially. The event informs the client * that many selection changes have occurred, and it is sent instead of several * EVENT_OBJECT_SELECTIONADD or EVENT_OBJECT_SELECTIONREMOVE events. The client * queries for the selected items by calling the container object's IAccessible::get_accSelection method and * enumerating the selected items. For this event notification, the hwnd and idObject parameters of the WinEventProc callback * function describe the container in which the changes occurred. */
        EVENT_OBJECT_SELECTIONWITHIN = 0x8009,

        /** A hidden object is shown. The system sends this event for the following user interface elements: caret, cursor, and window
    * object. Server applications send this event for their accessible objects. * Clients assume that when this event is sent by a parent object, all child objects are already displayed. * Therefore, server applications do not send this event for the child objects. * Hidden objects include the STATE_SYSTEM_INVISIBLE flag; shown objects do not include this flag. * The EVENT_OBJECT_SHOW event also indicates that the STATE_SYSTEM_INVISIBLE flag is cleared. Therefore, servers * do not send the EVENT_STATE_CHANGE event in this case. */
        EVENT_OBJECT_SHOW = 0x8002,

        /** An object's state has changed. The system sends this event for the following user interface elements: check box, combo box,
    * header control, push button, radio button, scroll bar, toolbar control, tree view control, up-down control, and window object. * Server applications send this event for their accessible objects. * For example, a state change occurs when a button object is clicked or released, or when an object is enabled or disabled. * For this event notification, the idChild parameter of the WinEventProc callback function identifies the child object whose state has changed. */
        EVENT_OBJECT_STATECHANGE = 0x800A,

        /** The conversion target within an IME composition has changed. The conversion target is the subset of the IME composition
    * which is actively selected as the target for user-initiated conversions. */
        EVENT_OBJECT_TEXTEDIT_CONVERSIONTARGETCHANGED = 0x8030,

        /** An object's text selection has changed. This event is supported by common controls and is used by UI Automation.
    * The hwnd, ID, and idChild parameters of the WinEventProc callback function describe the item that is contained in the updated text selection. */
        EVENT_OBJECT_TEXTSELECTIONCHANGED = 0x8014,

        /** Sent when a window is uncloaked. A cloaked window still exists, but is invisible to the user.
*/
        EVENT_OBJECT_UNCLOAKED = 0x8018,

        /** An object's Value property has changed. The system sends this event for the user interface elements that include the scroll
    * bar and the following controls: edit, header, hot key, progress bar, slider, and up-down. Server applications send this event * for their accessible objects. */
        EVENT_OBJECT_VALUECHANGE = 0x800E,

        /** The range of event constant values reserved for OEMs. For more information, see Allocation of WinEvent IDs.
*/
        EVENT_OEM_DEFINED_START = 0x0101,
        EVENT_OEM_DEFINED_END = 0x01FF,

        /** An alert has been generated. Server applications should not send this event.
*/
        EVENT_SYSTEM_ALERT = 0x0002,

        /** A preview rectangle is being displayed.
*/
        EVENT_SYSTEM_ARRANGMENTPREVIEW = 0x8016,

        /** A window has lost mouse capture. This event is sent by the system, never by servers.
*/
        EVENT_SYSTEM_CAPTUREEND = 0x0009,

        /** A window has received mouse capture. This event is sent by the system, never by servers.
*/
        EVENT_SYSTEM_CAPTURESTART = 0x0008,

        /** A window has exited context-sensitive Help mode. This event is not sent consistently by the system.
*/
        EVENT_SYSTEM_CONTEXTHELPEND = 0x000D,

        /** A window has entered context-sensitive Help mode. This event is not sent consistently by the system.
*/
        EVENT_SYSTEM_CONTEXTHELPSTART = 0x000C,

        /** The active desktop has been switched.
*/
        EVENT_SYSTEM_DESKTOPSWITCH = 0x0020,

        /** A dialog box has been closed. The system sends this event for standard dialog boxes; servers send it for custom dialog boxes.
    * This event is not sent consistently by the system. */
        EVENT_SYSTEM_DIALOGEND = 0x0011,

        /** A dialog box has been displayed. The system sends this event for standard dialog boxes, which are created using resource
    * templates or Win32 dialog box functions. Servers send this event for custom dialog boxes, which are windows that function as * dialog boxes but are not created in the standard way. * This event is not sent consistently by the system. */
        EVENT_SYSTEM_DIALOGSTART = 0x0010,

        /** An application is about to exit drag-and-drop mode. Applications that support drag-and-drop operations must send this event;
    * the system does not send this event. */
        EVENT_SYSTEM_DRAGDROPEND = 0x000F,

        /** An application is about to enter drag-and-drop mode. Applications that support drag-and-drop operations must send this
    * event because the system does not send it. */
        EVENT_SYSTEM_DRAGDROPSTART = 0x000E,

        /** The highest system event value.
*/
        EVENT_SYSTEM_END = 0x00FF,

        /** The foreground window has changed. The system sends this event even if the foreground window has changed to another window
    * in the same thread. Server applications never send this event. * For this event, the WinEventProc callback function's hwnd parameter is the handle to the window that is in the * foreground, the idObject parameter is OBJID_WINDOW, and the idChild parameter is CHILDID_SELF. */
        EVENT_SYSTEM_FOREGROUND = 0x0003,

        /** A pop-up menu has been closed. The system sends this event for standard menus; servers send it for custom menus.
    * When a pop-up menu is closed, the client receives this message, and then the EVENT_SYSTEM_MENUEND event. * This event is not sent consistently by the system. */
        EVENT_SYSTEM_MENUPOPUPEND = 0x0007,

        /** A pop-up menu has been displayed. The system sends this event for standard menus, which are identified by HMENU, and are
    * created using menu-template resources or Win32 menu functions. Servers send this event for custom menus, which are user * interface elements that function as menus but are not created in the standard way. This event is not sent consistently by the system. */
        EVENT_SYSTEM_MENUPOPUPSTART = 0x0006,

        /** A menu from the menu bar has been closed. The system sends this event for standard menus; servers send it for custom menus.
    * For this event, the WinEventProc callback function's hwnd, idObject, and idChild parameters refer to the control * that contains the menu bar or the control that activates the context menu. The hwnd parameter is the handle to the window * that is related to the event. The idObject parameter is OBJID_MENU or OBJID_SYSMENU for a menu, or OBJID_WINDOW for a * pop-up menu. The idChild parameter is CHILDID_SELF. */
        EVENT_SYSTEM_MENUEND = 0x0005,

        /** A menu item on the menu bar has been selected. The system sends this event for standard menus, which are identified
    * by HMENU, created using menu-template resources or Win32 menu API elements. Servers send this event for custom menus, * which are user interface elements that function as menus but are not created in the standard way. * For this event, the WinEventProc callback function's hwnd, idObject, and idChild parameters refer to the control * that contains the menu bar or the control that activates the context menu. The hwnd parameter is the handle to the window * related to the event. The idObject parameter is OBJID_MENU or OBJID_SYSMENU for a menu, or OBJID_WINDOW for a pop-up menu. * The idChild parameter is CHILDID_SELF.The system triggers more than one EVENT_SYSTEM_MENUSTART event that does not always * correspond with the EVENT_SYSTEM_MENUEND event. */
        EVENT_SYSTEM_MENUSTART = 0x0004,

        /** A window object is about to be restored. This event is sent by the system, never by servers.
*/
        EVENT_SYSTEM_MINIMIZEEND = 0x0017,

        /** A window object is about to be minimized. This event is sent by the system, never by servers.
*/
        EVENT_SYSTEM_MINIMIZESTART = 0x0016,

        /** The movement or resizing of a window has finished. This event is sent by the system, never by servers.
*/
        EVENT_SYSTEM_MOVESIZEEND = 0x000B,

        /** A window is being moved or resized. This event is sent by the system, never by servers.
*/
        EVENT_SYSTEM_MOVESIZESTART = 0x000A,

        /** Scrolling has ended on a scroll bar. This event is sent by the system for standard scroll bar controls and for
    * scroll bars that are attached to a window. Servers send this event for custom scroll bars, which are user interface * elements that function as scroll bars but are not created in the standard way. * The idObject parameter that is sent to the WinEventProc callback function is OBJID_HSCROLL for horizontal scroll bars, and * OBJID_VSCROLL for vertical scroll bars. */
        EVENT_SYSTEM_SCROLLINGEND = 0x0013,

        /** Scrolling has started on a scroll bar. The system sends this event for standard scroll bar controls and for scroll
    * bars attached to a window. Servers send this event for custom scroll bars, which are user interface elements that * function as scroll bars but are not created in the standard way. * The idObject parameter that is sent to the WinEventProc callback function is OBJID_HSCROLL for horizontal scrolls bars, * and OBJID_VSCROLL for vertical scroll bars. */
        EVENT_SYSTEM_SCROLLINGSTART = 0x0012,

        /** A sound has been played. The system sends this event when a system sound, such as one for a menu,
    * is played even if no sound is audible (for example, due to the lack of a sound file or a sound card). * Servers send this event whenever a custom UI element generates a sound. * For this event, the WinEventProc callback function receives the OBJID_SOUND value as the idObject parameter. */
        EVENT_SYSTEM_SOUND = 0x0001,

        /** The user has released ALT+TAB. This event is sent by the system, never by servers.
    * The hwnd parameter of the WinEventProc callback function identifies the window to which the user has switched. * If only one application is running when the user presses ALT+TAB, the system sends this event without a corresponding * EVENT_SYSTEM_SWITCHSTART event. */
        EVENT_SYSTEM_SWITCHEND = 0x0015,

        /** The user has pressed ALT+TAB, which activates the switch window. This event is sent by the system, never by servers.
    * The hwnd parameter of the WinEventProc callback function identifies the window to which the user is switching. * If only one application is running when the user presses ALT+TAB, the system sends an EVENT_SYSTEM_SWITCHEND event without a * corresponding EVENT_SYSTEM_SWITCHSTART event. */
        EVENT_SYSTEM_SWITCHSTART = 0x0014,

        /** The range of event constant values reserved for UI Automation event identifiers. For more information,
    * see Allocation of WinEvent IDs. */
        EVENT_UIA_EVENTID_START = 0x4E00,
        EVENT_UIA_EVENTID_END = 0x4EFF,

        /**
    * The range of event constant values reserved for UI Automation property-changed event identifiers. * For more information, see Allocation of WinEvent IDs. */
        EVENT_UIA_PROPID_START = 0x7500,
        EVENT_UIA_PROPID_END = 0x75FF
    }

    [Flags]
    public enum WinEventFlags : uint
    {
        WINEVENT_OUTOFCONTEXT = 0x0000, // Events are ASYNC
        WINEVENT_SKIPOWNTHREAD = 0x0001, // Don't call back for events on installer's thread
        WINEVENT_SKIPOWNPROCESS = 0x0002, // Don't call back for events on installer's process
        WINEVENT_INCONTEXT = 0x0004, // Events are SYNC, this causes your dll to be injected into every process
    }

    public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType,
        IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

    [DllImport("user32.dll")]
    public static extern IntPtr SetWinEventHook(WinEvents eventMin, WinEvents eventMax, IntPtr
            hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess,
        uint idThread, WinEventFlags dwFlags);

    [DllImport("user32.dll")]
    public static extern bool UnhookWinEvent(IntPtr hWinEventHook);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    [Flags()]
    public enum SetWindowPosFlags : uint
    {
        /// <summary>If the calling thread and the thread that owns the window are attached to different input queues,
        /// the system posts the request to the thread that owns the window. This prevents the calling thread from
        /// blocking its execution while other threads process the request.</summary>
        /// <remarks>SWP_ASYNCWINDOWPOS</remarks>
        AsynchronousWindowPosition = 0x4000,
        /// <summary>Prevents generation of the WM_SYNCPAINT message.</summary>
        /// <remarks>SWP_DEFERERASE</remarks>
        DeferErase = 0x2000,
        /// <summary>Draws a frame (defined in the window's class description) around the window.</summary>
        /// <remarks>SWP_DRAWFRAME</remarks>
        DrawFrame = 0x0020,
        /// <summary>Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to
        /// the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE
        /// is sent only when the window's size is being changed.</summary>
        /// <remarks>SWP_FRAMECHANGED</remarks>
        FrameChanged = 0x0020,
        /// <summary>Hides the window.</summary>
        /// <remarks>SWP_HIDEWINDOW</remarks>
        HideWindow = 0x0080,
        /// <summary>Does not activate the window. If this flag is not set, the window is activated and moved to the
        /// top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter
        /// parameter).</summary>
        /// <remarks>SWP_NOACTIVATE</remarks>
        DoNotActivate = 0x0010,
        /// <summary>Discards the entire contents of the client area. If this flag is not specified, the valid
        /// contents of the client area are saved and copied back into the client area after the window is sized or
        /// repositioned.</summary>
        /// <remarks>SWP_NOCOPYBITS</remarks>
        DoNotCopyBits = 0x0100,
        /// <summary>Retains the current position (ignores X and Y parameters).</summary>
        /// <remarks>SWP_NOMOVE</remarks>
        IgnoreMove = 0x0002,
        /// <summary>Does not change the owner window's position in the Z order.</summary>
        /// <remarks>SWP_NOOWNERZORDER</remarks>
        DoNotChangeOwnerZOrder = 0x0200,
        /// <summary>Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to
        /// the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent
        /// window uncovered as a result of the window being moved. When this flag is set, the application must
        /// explicitly invalidate or redraw any parts of the window and parent window that need redrawing.</summary>
        /// <remarks>SWP_NOREDRAW</remarks>
        DoNotRedraw = 0x0008,
        /// <summary>Same as the SWP_NOOWNERZORDER flag.</summary>
        /// <remarks>SWP_NOREPOSITION</remarks>
        DoNotReposition = 0x0200,
        /// <summary>Prevents the window from receiving the WM_WINDOWPOSCHANGING message.</summary>
        /// <remarks>SWP_NOSENDCHANGING</remarks>
        DoNotSendChangingEvent = 0x0400,
        /// <summary>Retains the current size (ignores the cx and cy parameters).</summary>
        /// <remarks>SWP_NOSIZE</remarks>
        IgnoreResize = 0x0001,
        /// <summary>Retains the current Z order (ignores the hWndInsertAfter parameter).</summary>
        /// <remarks>SWP_NOZORDER</remarks>
        IgnoreZOrder = 0x0004,
        /// <summary>Displays the window.</summary>
        /// <remarks>SWP_SHOWWINDOW</remarks>
        ShowWindow = 0x0040,
    }


    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr FindWindow(string? lpClassName, string lpWindowName);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
    
}
