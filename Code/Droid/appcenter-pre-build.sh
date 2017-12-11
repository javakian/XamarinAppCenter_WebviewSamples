#!/bin/bash

find .. -name Keys.cs -exec sed -i -e 's/AndroidKey/'"$ANDROID_APPCENTER_KEY"'/g' {} \;