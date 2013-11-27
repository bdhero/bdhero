[Code]
// Adapted from http://www.vincenzo.net/isxkb/index.php?title=Ask_for_a_drive_to_install

function GetDriveType(lpDisk: String): Integer;
external 'GetDriveTypeW@kernel32.dll stdcall';

function GetLogicalDriveStrings(nLenDrives: LongInt; lpDrives: String): Integer;
external 'GetLogicalDriveStringsW@kernel32.dll stdcall';

const
    DRIVE_UNKNOWN     = 0; // The drive type cannot be determined.
    DRIVE_NO_ROOT_DIR = 1; // The root path is invalid. For example, no volume is mounted at the path.
    DRIVE_REMOVABLE   = 2; // The disk can be removed from the drive.
    DRIVE_FIXED       = 3; // The disk cannot be removed from the drive.
    DRIVE_REMOTE      = 4; // The drive is a remote (network) drive.
    DRIVE_CDROM       = 5; // The drive is a CD-ROM drive.
    DRIVE_RAMDISK     = 6; // The drive is a RAM disk.

var
    // Combo box for drives
    driveComboBox: TComboBox;

    // Array of removable drive paths.
    // E.G.: [ "E:\", "F:\" ]
    removableDrivePaths: Array of String;

// Function to convert disk type to a recognizable string.
function DriveTypeString(driveType: Integer): String;
begin
    case driveType of
        DRIVE_NO_ROOT_DIR : Result := 'Root path invalid';
        DRIVE_REMOVABLE   : Result := 'Removable';
        DRIVE_FIXED       : Result := 'Fixed';
        DRIVE_REMOTE      : Result := 'Network';
        DRIVE_CDROM       : Result := 'CD-ROM';
        DRIVE_RAMDISK     : Result := 'RAM disk';
    else
        Result := 'Unknown';
    end;
end;

// NOTE: This function will not work on Windows 8+ because USB flash drives are no longer reported as "removable".
// See http://kb.sandisk.com/app/answers/detail/a_id/12830
function GetFirstRemovableDrive(): String;
var
    // List of all drives and their types (for debugging purposes).
    // E.G.:
    //     C:\ = Fixed
    //     D:\ = CD-ROM
    //     E:\ = Fixed
    driveTypeList: String;

    // E.G.: "C:\"
    systemDrivePath: String;

    // String buffer containing a series of null-terminated strings,
    // one for each valid drive in the system, plus an additional null character.
    // E.G.: "C:\\0D:\\0E:\\0\\0" -> [ "C:\", "D:\", "E:\" ]
    driveStringsBuf: String;
    driveStringsLen: Integer;

    idx: Integer;

    curNullPos: Integer;

    // E.G.: "C:\"
    curDrivePath: String;
    curDriveType: Integer;

begin
    driveTypeList := '';

    // Get the system drive
    systemDrivePath := UpperCase(ExpandConstant('{sd}')) + '\';

    // Get all drive letters
    driveStringsBuf := StringOfChar(' ', 64);
    driveStringsLen := GetLogicalDriveStrings(63, driveStringsBuf);
    SetLength(driveStringsBuf, driveStringsLen);

    idx := 0;

    while Length(driveStringsBuf) > 0 do
    begin
        curNullPos := Pos(#0, driveStringsBuf);

        if curNullPos > 0 then
        begin
            curDrivePath := UpperCase(Copy(driveStringsBuf, 1, curNullPos - 1));
            curDriveType := GetDriveType(curDrivePath);

            if curDriveType = DRIVE_REMOVABLE then
            begin
                SetArrayLength(removableDrivePaths, idx + 1);
                removableDrivePaths[idx] := curDrivePath;

                // Default combobox selection to the user's system drive
                //if (curDrivePath = systemDrivePath) then driveComboBox.ItemIndex := idx;

                idx := idx + 1;
            end;

            driveTypeList := driveTypeList + curDrivePath + ' = ' + DriveTypeString(curDriveType) + #13;

            driveStringsBuf := Copy(driveStringsBuf, curNullPos + 1, Length(driveStringsBuf));
        end;
    end;

#ifdef DebugMode
    MsgBox(driveTypeList, mbInformation, MB_OK);
#endif

    if (idx > 0) then
        Result := removableDrivePaths[idx - 1]
    else
        Result := systemDrivePath
end;
