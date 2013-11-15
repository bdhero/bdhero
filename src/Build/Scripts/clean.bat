@ECHO OFF

REM %CD% = C:\Projects\BDHero

git status

git reset --hard HEAD
git checkout master
git pull origin master
git status
git log -n 5
