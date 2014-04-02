; Adapted from http://www.codeproject.com/Articles/20868/NET-Framework-1-1-2-0-3-5-Installer-for-InnoSetup

;#define DebugMode

#define MyAppName "BDHero"
#define MyAppMachineName "bdhero"
#define MyAppVersion "0.9.0.2"
#define MyAppPublisher "BDHero"
#define MyAppURL "http://bdhero.org/"
#define MyAppExeName "bdhero-gui.exe"

#define DefaultInstallDir "{localappdata}\" + MyAppName + "\Application"
#define LogDir "{localappdata}\" + MyAppName + "\Logs"
#define PluginDir "{app}\Plugins"

#define CodeSigningCertPK GetEnv('CodeSigningCertPK')
#define CodeSigningCertPW GetEnv('CodeSigningCertPW')

#define InstallerArtifactDir "..\..\..\artifacts\Installer"
#define DeleteFileFlags "uninsrestartdelete ignoreversion"
#define DeleteDirFlags "uninsrestartdelete ignoreversion createallsubdirs recursesubdirs"

#include "dependencies.iss"
#include "install-dir.iss"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={#MyAppName}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
VersionInfoVersion={#MyAppVersion}
AppVerName={#MyAppName}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}

;SourceDir=..\..\
OutputDir=..\..\..\artifacts
OutputBaseFilename={#MyAppMachineName}-{#MyAppVersion}-setup

WizardImageFile=..\..\..\assets\InnoSetup\bdrom_wizard_2.bmp
WizardSmallImageFile=..\..\..\assets\InnoSetup\bdhero_gui_55x58.bmp

MinVersion=0,5.01sp3
PrivilegesRequired=lowest

Uninstallable=not IsPortable
UninstallDisplayIcon={app}\{#MyAppExeName}

; "ArchitecturesInstallIn64BitMode=x64" requests that the install be
; done in "64-bit mode" on x64, meaning it should use the native
; 64-bit Program Files directory and the 64-bit view of the registry.
; On all other architectures it will install in "32-bit mode".
ArchitecturesInstallIn64BitMode=x64
; Note: We don't set ProcessorsAllowed because we want this
; installation to run on all architectures (including Itanium,
; since it's capable of running 32-bit code too).

UsePreviousAppDir=yes
DefaultDirName={#DefaultInstallDir}
AppendDefaultDirName=no
DefaultGroupName={#MyAppName}
AllowNoIcons=yes

DisableWelcomePage=yes
DisableDirPage=no
DisableProgramGroupPage=yes
AlwaysShowDirOnReadyPage=yes
AlwaysShowGroupOnReadyPage=no

#ifdef DebugMode
    Compression=none
#else
    Compression=lzma2/ultra64
    SolidCompression=True
    InternalCompressLevel=ultra64
    
    #if CodeSigningCertPK != ""
        SignTool=Custom sign /v /f {#CodeSigningCertPK} /p {#CodeSigningCertPW} /d $q{#MyAppName} Setup$q /du $q{#MyAppURL}$q /t http://timestamp.comodoca.com/authenticode $f
        SignedUninstaller=True
    #endif
#endif

ShowLanguageDialog=auto

[Languages]
Name: "en"; MessagesFile: "compiler:Default.isl"
Name: "de"; MessagesFile: "compiler:Default.isl"

[Messages]
SetupWindowTitle=Setup - %1 {#MyAppVersion}

[CustomMessages]
en.InstallationTypeNormal=&Normal - PC Hard Disk (current user only)
en.InstallationTypeUpgrade=&Upgrade - PC Hard Disk (current user only)
en.InstallationTypePortable=&Portable - USB Thumb Drive

;[Tasks]
;Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "{#InstallerArtifactDir}\ProgramFiles\bdhero-gui.exe"; DestDir: "{app}";                  Flags: {#DeleteFileFlags}
Source: "{#InstallerArtifactDir}\ProgramFiles\*";              DestDir: "{app}";                  Flags: {#DeleteDirFlags}
Source: "{#InstallerArtifactDir}\Plugins\Required\*";          DestDir: "{#PluginDir}\Required";  Flags: {#DeleteDirFlags}
Source: "{#InstallerArtifactDir}\Config\*";                    DestDir: "{code:ConfigDirAuto}";   Flags: {#DeleteDirFlags}

[UninstallDelete]
Type: filesandordirs; Name: "{#LogDir}"
Type: filesandordirs; Name: "{#PluginDir}"
Type: dirifempty;     Name: "{code:ConfigDirAuto}\Application"
Type: dirifempty;     Name: "{code:ConfigDirAuto}"
Type: dirifempty;     Name: "{userappdata}\{#MyAppName}"
Type: dirifempty;     Name: "{localappdata}\{#MyAppName}"
Type: dirifempty;     Name: "{app}"

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Check: not IsPortable
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"; Check: not IsPortable
;Name: "{userdesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon; Check: not IsPortable

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Code]
function InitializeSetup(): Boolean;
begin
	InitializeSetupDeps();
	Result := true;
end;

procedure InitializeWizard;
begin
    InitializeWizardInstallType();

#ifdef DebugMode
    if IsAlreadyInstalled() then
        MsgBox('Already installed in "' + WizardForm.DirEdit.Text + '"', mbInformation, MB_OK)
    else
        MsgBox('NOT already installed (checked "' + WizardForm.DirEdit.Text + '")', mbInformation, MB_OK)
#endif
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
var
    response: Integer;
begin
    case CurUninstallStep of
        usUninstall:
            begin
                response := MsgBox('Remove preferences too?', mbConfirmation, MB_YESNO or MB_DEFBUTTON2)
                if response = IDYES then
                    DelTree(ConfigDirAuto(''), True, True, True);
            end;
    end;
end;

function NextButtonClick(CurPageID: Integer): Boolean;
begin
    Result := true;

    if CurPageID = pInstallationTypePage.ID then
        RestoreInstallDirAuto()
    else
        Result := NextButtonClickCheckPrereq(CurPageID)
end;

function ShouldSkipPage(PageID: Integer): Boolean;
begin
    if (PageID = wpSelectDir) and (not IsPortable()) and IsAlreadyInstalled() then
        Result := true
    else
        Result := false
    ;
end;
