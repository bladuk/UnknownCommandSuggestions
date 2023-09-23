# UnknownCommandSuggestions ![Downloads](https://img.shields.io/github/downloads/bladuk/UnknownCommandSuggestions/total.svg)
SCP: SL plugin which, if an administrator/player executes an unknown command, will try to suggest possible correct input options.

## How to install plugin?
Put UnknownCommandSuggestions.dll under the release tab into %appdata%\EXILED\Plugins (Windows) or .config/EXILED/Plugins (Linux) folder.

## Default configs
```yaml
unknowncommandsuggestions:
  # Is the plugin enabled?
  is_enabled: true
  # Show debug messages in the server console?
  debug: false
  # String displayed before the suggestions
  unknown_command_reply: 'Maybe you meant:\n'
```
