# gofer

Gofer is a simple database migration CLI utility.

## Usage

### Create

```shell
create:
  Create a database

Usage:
  gofer create [options] <database>

Arguments:
  <database>    Specify the name of the database to be created

Options:
  -c, --connection-string <connection-string>                            Specify SQL Server connection string
  -f, --force                                                            Force delete of database if it already exists
  -v, --verbosity                                                        Set output verbosity [default: Warning]
  <Critical|Debug|Error|Information|None|Trace|Warning>
  -?, -h, --help                                                         Show help and usage information
```

### Migrate

```shell
migrate:
  Perform database migrations

Usage:
  gofer migrate [options] [<migrations-directory>]

Arguments:
  <migrations-directory>    Path to migration scripts (leave blank for current directory)

Options:
  -c, --connection-string <connection-string>                 Specify SQL Server connection string
  -d, --dryrun                                                Perform a dry-run instead of executing migration scripts
  -j, --journal <journal>                                     Specify journal schema and name [default:
                                                              dbo._MigrationJournal]
  -l, --limit <limit>                                         Specify a limit on the amount of scripts to execute from
                                                              the current position in the migration journal. A
                                                              negative number will run all but N
  -o, --output <output>                                       Specify custom path for the HTML report
  -p, --pretend                                               Journal scripts without actually executing content
  -r, --report                                                Generate an HTML report of the scripts to be executed
                                                              during the migration
  -v, --verbosity                                             Set output verbosity [default: Warning]
  <Critical|Debug|Error|Information|None|Trace|Warning>
  -?, -h, --help                                              Show help and usage information

```

## Dependencies

The following libraries are used in this tool:

- [System.CommandLine](https://github.com/dotnet/command-line-api)
- [DbUp](https://github.com/DbUp/DbUp)
- [MediatR](https://github.com/jbogard/MediatR)
- [Serilog](https://github.com/serilog/serilog)

## License

MIT
<!--[Mammals Vectors by Vecteezy](https://www.vecteezy.com/free-vector/mammals)-->
