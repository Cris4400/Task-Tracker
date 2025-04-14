# Task-Tracker

Task-Tracker is a command-line interface (CLI) tool that enables users to create, read, update, and delete tasks stored in a JSONL file. With attributes like ID, description, status, and timestamps, it provides a simple and efficient way to manage personal tasks. 
This project was created by following the specifications from [roadmap.sh's Task Tracker project](https://roadmap.sh/projects/task-tracker).

## Overview

Each task in Task-Tracker is a JSON object with the following attributes:

- **ID**: A unique number identifying the task.
- **Description**: A brief text summarizing the task.
- **Status**: Indicates the task's current phase:
  - `todo`: Task not started.
  - `in-progress`: Task being worked on.
  - `done`: Task completed.
- **CreatedAt**: The date and time when the task was created.
- **UpdatedAt**: The date and time of the last update. Displays `N/A` if never updated.

## Requirements

- .NET 8.0 SDK or later
- A terminal or command prompt
- Write permissions in the application data directory:
  - Windows: `%APPDATA%`
  - Linux: `~/.config`
  - macOS: `~/Library/Application Support`

## Installation

To use Task-Tracker, download the project, pack it as a .NET tool, and install it globally or locally. The global installation is recommended for access from any terminal. Follow these steps:

### 1. Download the project

Clone the repository:  

`git clone https://github.com/Cris4400/Task-Tracker.git`

Alternatively, download the ZIP file from the [GitHub repository](https://github.com/Cris4400/Task-Tracker) and extract it.

### 2. Navigate to the project folder

`cd Task-Tracker`

### 3. Verify the .csproj configuration

Ensure the `Task-Tracker.csproj` file includes:

`<PackAsTool>true</PackAsTool>`  
`<ToolCommandName>task-cli</ToolCommandName>`  
`<PackageOutputPath>./nupkg</PackageOutputPath>`

These settings:
- Enable the project to be packed as a .NET tool.
- Set the CLI command to `task-cli`.
- Store the NuGet package in the `nupkg` folder.

### 4. Pack the tool

Pack the project into a NuGet package:  

`dotnet pack`

### 5. Install the tool globally

Install the tool globally to make `task-cli` available from any terminal:  

`dotnet tool install --global --add-source ./nupkg Task-Tracker`

Run this command from the project folder.

### 6. Verify installation

Confirm the tool is installed correctly:  

`task-cli help`

This should display the available commands and usage information.

### 7. Local installation (optional)

For local installation, see [Microsoft's guide on local tools](https://learn.microsoft.com/en-us/dotnet/core/tools/local-tools-how-to-use).

### 8. Uninstall the tool

To remove the global tool:  

`dotnet tool uninstall --global Task-Tracker`

## Usage

Use the `task-cli` command to manage tasks with the following syntax:  

`task-cli <command> [arguments]`

### Commands

- `add <description>` – Creates a new task with the specified description.
- `update <id> <desc>` – Updates the description of the task with the given ID.
- `delete <id>` – Deletes the task with the specified ID.
- `mark-in-progress <id>` – Marks the task with the given ID as 'in progress'.
- `mark-done <id>` – Marks the task with the given ID as 'done'.
- `list` – Displays all tasks.
- `list done` – Shows only completed tasks.
- `list todo` – Shows tasks that are still to be done.
- `list in-progress` – Shows tasks currently in progress.
- `help` – Displays this help message.
- `help <command>` – Shows detailed help for a specific command.

### Examples

`task-cli add "Buy groceries"`  
`task-cli update 3 "Finish project"`  
`task-cli delete 2`  
`task-cli mark-in-progress 5`  
`task-cli mark-done 8`  
`task-cli list`  
`task-cli list done`  
`task-cli help add`

## Data Storage

Tasks are stored in a `data.jsonl` file within a `task-tracker` subdirectory in the application's data directory:  

- Windows: `%APPDATA%\task-tracker\data.jsonl`  
- Linux: `~/.config/task-tracker/data.jsonl`  
- macOS: `~/Library/Application Support/task-tracker/data.jsonl`

The `task-tracker` directory is created automatically on first use. Each line in `data.jsonl` is a JSON object containing a task’s ID, description, status, and timestamps. You can inspect or edit this file manually, but use the CLI to avoid formatting errors.

## How It Works

Task-Tracker uses a JSONL file for efficient task storage:  

- Adding a task: Appends a new JSON object to the end of `data.jsonl`.  
- Updating or deleting a task: Reads all lines into memory, modifies or removes the task by ID, and rewrites the file.  
- Listing tasks: Reads the file and filters tasks based on the requested criteria.

This approach ensures simplicity and is suitable for small to medium task lists. For large files, performance could be optimized by avoiding full file rewrites when modifying the last line, but this is not critical for typical usage.

## Troubleshooting

- **"Permission denied"**:  
  Ensure you have write access to the application data directory. On Windows, try running the terminal as administrator.
  

- **"`task-cli` not found"**:  
  First, verify the tool is installed globally:

    dotnet tool list --global

  If it’s not listed, reinstall it using:

    dotnet tool install --global --add-source ./nupkg Task-Tracker

  On **Linux**, you might also need to manually add the `.dotnet/tools` directory to your `$PATH`. You can do this by running:

    `export PATH="$PATH:$HOME/.dotnet/tools"`

  To make it permanent, add the following line to your shell's profile file:

  - For **Bash**: `~/.bashrc`
  - For **Zsh**: `~/.zshrc`
  - For **Fish**: `~/.config/fish/config.fish`

  Example (Bash):
    ```bash
    echo 'export PATH="$PATH:$HOME/.dotnet/tools"' >> ~/.bashrc
    source ~/.bashrc
    ```

  After this, `task-cli` should work from any terminal session.

- **Missing `data.jsonl`**:  
  Don’t worry — the file is created automatically the first time you add a task.

- **Invalid command or ID**:  
  Use `task-cli help` to check available commands or confirm the task ID exists.
