BDHero Changelog
================

0.9.0.8 - 2014-04-16
--------------------

*   Fix: Ignore "Last message repeated X times" message from FFmpeg

0.9.0.7 - 2014-04-15
--------------------

*   Fix: #28: ChapterDb serialization exception when parsing invalid results
*   Fix: Taskbar pinning broke between upgrades
*   Fix: Logic bug in `ExceptionUtils.IsReportable()`
*   Improve: Alert user if she tries to scan an encrypted disc
*   Improve: Added PDB files to provide line numbers in error reports
*   Improve: More detailed logging
*   Misc: Added `LICENSE-GPL.txt`

0.9.0.6 - 2014-04-07
--------------------

*   Fix: #19 and #21: Number parsing bug with non-US cultures
*   Fix: #22: Invalid exception (due to user error) could be reported
*   Improve: Better exception handling in FFmpeg muxer plugin
*   Improve: Better log output

0.9.0.5 - 2014-04-04
--------------------

*   New: Strip leading chapter numbers from chapter names
*   Improve: Better log output
*   Improve: More detailed error reports

0.9.0.4 - 2014-04-02
--------------------

*   Fix: #15, #17, and #18: `GetJacketImage()` throws NPE when
    `Directories.BDMT` is `null`
*   Fix: #16: `IEnumerable.First()` throws `InvalidOperationException` when list
    contains no elements
*   Fix: CLI thread synchronization exception
*   Change: Default log level is now `INFO` (was previously `DEBUG`)
*   Improve: Better (shorter) CLI output
*   Minor improvements

0.9.0.3 - 2014-04-02
--------------------

*   Fix: Error dialog thread synchronization bug on Windows 7 and newer
*   Minor improvements

0.9.0.2 - 2014-04-02
--------------------

*   Fix: <kbd>Shift</kbd> + <kbd>Tab</kbd> didn't work if TextEditor was the
    first input
*   Fix: <kbd>Ctrl</kbd> + <kbd>I</kbd> shortcut didn't work in input/output
    path textboxes
*   Fix: Hide non-functional UI components
*   Change: Updated placeholder variable format from `${res}` to `{res}`
    to avoid conflicts with environment variables on *NIX platforms
*   Improve: Expand environment variables in input/output path textboxes
*   Improve: Progress bar colors

0.9.0.1 - 2014-03-31
--------------------

*   Fix: Removed debugging code

0.9.0.0 - 2014-03-31
--------------------

*   Beta release
