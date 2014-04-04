BDHero Changelog
================

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
