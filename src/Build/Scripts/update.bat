@ECHO OFF

REM %CD% = C:\Projects\BDHero\src

call Build\Scripts\tools.bat

Build\Tools\UMGen --mirror "%MirrorUrl%" --windows --setup "%SetupPath%" --portable "%ZipPath%" --output update.json
