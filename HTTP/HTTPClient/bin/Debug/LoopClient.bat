    @echo off
    setlocal enableextensions enabledelayedexpansion
    set /a "x = 0"

:more_to_process
    if %x% leq 3 (
        start  HTTPClient.exe %x% 127.0.0.1 7999
        set /a "x = x + 1"
        goto :more_to_process
    )

    