#!/usr/bin/env bash

declare -i SUCCESS=0
declare -i MISSING_VERSION_IN_LABEL_STATUS=1
declare -i MISSING_VERSION_IN_CSPROJ_STATUS=2
declare -i VERSIONS_ARE_NOT_EQUAL_STATUS=2

declare labels="$1"

VERSION="$(echo -e "$labels" | grep 'v.*' | head -n 1)"
if [[ -z "$VERSION" ]]
then
    echo "Missing label with version specified for pull request."
    exit "$MISSING_VERSION_IN_LABEL_STATUS"
fi

if [[ "$(cat "$CSPROJ_PATH")" =~ \<Version\>(.*)\</Version\> ]]
then
    CSPROJ_VERSION="${BASH_REMATCH[1]}"
    [[ "$CSPROJ_VERSION" == "$VERSION" ]] || {
        echo "Version specified with pull request label ($VERSION) is not equal to version specified in C# project ($CSPROJ_VERSION)."
        exit "$VERSIONS_ARE_NOT_EQUAL_STATUS"
    }
else
    echo "Missing label with version specified for C# project."
    exit "$MISSING_VERSION_IN_CSPROJ_STATUS"
fi

echo "VERSION=$VERSION" >> "$GITHUB_ENV"
exit "$SUCCESS"
