# Prerequisits
This repo uses `pre-commit` to allow a versioned list of all commit hooks that might be used in the future
This includes:
- pre-commit checks (checks that run before you are allowed to make a commit message)
- commit-msg checks (checks that make changes to the commit message automatically - TBI)

## How to install
Verify your install with `pre-commit --version` after following the instructions
### Linux
Install `pre-commit` using pip (python package manager)
```sh
# Using pip (Recommended)
pip install pre-commit

# Alternatively, using pipx
pipx install pre-commit

# Alternatively, on Ubuntu/Debian
sudo apt update && sudo apt install pre-commit
```


### Windows
Install `pre-commit` using powershell or command prompt
```sh
# Using pip
pip install pre-commit

# Alternatively, using winget
winget install pre-commit


```
# Using the pre-commit hooks
In order to install the pre-commit hooks, you can run the following in your terminal at the repo root directory
```sh
pre-commit install
```

## Manually running the hooks on all files
This can be done with
```sh
pre-commit run --all-files
```

# Updating the pre-commit hooks
This doesn't have to be done generally but anyway
```sh
pre-commit autoupdate
```

# Troubleshooting
## Windows complains
If windows complains about executing scripts, open Powershell as administrator and execute
```sh
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
```
## Bypassing the hooks temporarily
If anything ever goes wring with the hooks or you REALLY have to make an emergency commit, run
```sh
git commit -m "Emergency fix" --no-verify
```
But please don't do this too often and always discuss first
