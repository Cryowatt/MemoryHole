# MemoryHole

Repro from dotnet bug when running in Docker.

## How to run

In powershell core:

```powershell
./Start-Test.ps1
```

Optionally, you can add the `-Verbose` argument to show the docker build/run steps.

## What this application does

This application allocates memory in chunks of 32mb inside of a `MemoryFailPoint`. The expected behaviour is the `MemoryFailPoint` should
throw a `InsufficientMemoryException` _before_ the application would normally throw an `OutOfMemoryException`, allowing the application to
fail gracefully.

## Expected behaviour

When running natively in Windows the application will consume memory until all of the available physical and virtual memory are depleated.
The `MemoryFailPoint` throws an `InsufficientMemoryException` on the next iteration. The try/catch block catches the exception and the
application exits normally.  My expectation is that the dotnet runtime should function the same way in a docker container.

## Actual behaviour in a docker container

The `MemoryFailPoint` never throws an `InsufficientMemoryException` in a docker container. If run from a container that wasn't started with
a `--memory` argument set, the container is eventually killed with an exit code of `137`, but docker doesn't not report it as `OOMKilled`.
With `--memory` set, the container exits with `137` again, but with `OOMKilled: True`.
