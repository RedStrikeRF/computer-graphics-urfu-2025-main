#!/bin/bash

FOLDER="lab$1"
CS_FILES=$(find $FOLDER -type f -name "*.cs" -not -path "*/example/*")
echo "Обнаружены файлы: $CS_FILES"

REFS=()
for lib in $(find libs -type f -name "*.dll"); do
    REFS+=("-r:$lib")
done
echo "Обнаружены библиотеки: ${REFS[@]}"

output=$(
    mcs \
        -r:System.Windows.Forms.dll \
        -r:System.Drawing.dll \
        ${REFS[@]} \
        -out:Release.exe \
        $CS_FILES
)
echo "$output"

if [[ $output != *"Compilation failed"* ]]; then
    echo -e "\033[32mСборка завершена успешно. Файл: $FOLDER/Release.exe\033[0m"
    echo "Запуск..."
    mono Release.exe
fi

# Check Version
# monodis --assembly libs/OpenTK.dll

# glxinfo | grep "OpenGL version"