#!/bin/bash

find .. -name Keys.cs -exec sed -i -e 's/iOSKey/'"$IOS_APPCENTER_KEY"'/g' {} \;