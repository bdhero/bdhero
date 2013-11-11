[Code]
// http://www.vincenzo.net/isxkb/index.php?title=Ask_for_a_drive_to_install

var
	// combo box for drives
	cbDrive : TComboBox ;
	// array of strings that stores the drive letters
	DrvLetters: array of String;

function GetDriveType( lpDisk: String ): Integer;
external 'GetDriveTypeW@kernel32.dll stdcall';

function GetLogicalDriveStrings( nLenDrives: LongInt; lpDrives: String ): Integer;
external 'GetLogicalDriveStringsW@kernel32.dll stdcall';

const
  DRIVE_UNKNOWN = 0;     // The drive type cannot be determined.
  DRIVE_NO_ROOT_DIR = 1; // The root path is invalid. For example, no volume is mounted at the path.
  DRIVE_REMOVABLE = 2;   // The disk can be removed from the drive.
  DRIVE_FIXED = 3;       // The disk cannot be removed from the drive.
  DRIVE_REMOTE = 4;      // The drive is a remote (network) drive.
  DRIVE_CDROM = 5;       // The drive is a CD-ROM drive.
  DRIVE_RAMDISK = 6;     // The drive is a RAM disk.


// function to convert disk type to a regonizable string. This piece needs translation
// to other languages
function DriveTypeString( dtype: Integer ): String ;
begin
  case dtype of
    DRIVE_NO_ROOT_DIR : Result := 'Root path invalid';
    DRIVE_REMOVABLE : Result := 'Removable';
    DRIVE_FIXED : Result := 'Fixed';
    DRIVE_REMOTE : Result := 'Network';
    DRIVE_CDROM : Result := 'CD-ROM';
    DRIVE_RAMDISK : Result := 'Ram disk';
  else
    Result := 'Unknown';
  end;
end;

function GetFirstRemovableDrive() : String;
var
  n: Integer;
  drivesletters: String; lenletters: Integer;
  drive: String;
  disktype, posnull: Integer;
  sd: String;
  
begin
  //get the system drive
  sd := UpperCase(ExpandConstant('{sd}'));

  //get all drive letters of the system
  drivesletters := StringOfChar( ' ', 64 );
  lenletters := GetLogicalDriveStrings( 63, drivesletters );
  SetLength( drivesletters , lenletters );

  drive := '';
  n := 0;
  
  while ( (Length(drivesletters) > 0) ) do
  begin
    posnull := Pos( #0, drivesletters );
  	if posnull > 0 then
  	begin
      drive := UpperCase( Copy( drivesletters, 1, posnull - 1 ) );

      // get number type of disk
      disktype := GetDriveType( drive );

      // you can add various types of checks here to limit the types of drives that
      // are displayed to the user. in this example we add a drive only if is not a
      // removable drive (ie floppy, USB key, etc). you may add whatever limitations
      // you need in the next IF statement
      if ( disktype = DRIVE_REMOVABLE ) then
      begin
        SetArrayLength(DrvLetters, n + 1);
        DrvLetters[n] := drive;

        // default select C: Drive (not very wise since the users system drive may not be C: or they
        // may not even have a C: drive
        //if ( Copy(drive,1,2) = 'C:' ) then cbDrive.ItemIndex := n;
        // instead default it to the users system drive
        //if ( Copy(drive,1,2) = sd ) then cbDrive.ItemIndex := n;
        
        n := n + 1;
      end;
      
  	  drivesletters := Copy(drivesletters, posnull + 1, Length(drivesletters));
  	end;
  end;
  
  if ( n > 0 ) then
    Result := DrvLetters[n - 1]
  else
    Result := sd + '\'
end;