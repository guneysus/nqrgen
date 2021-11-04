function publish {
  dotnet publish nqrgen.csproj `
      -c Release `
      -r win-x64 `
      --self-contained true `
      -p:PublishTrimmed=true `
      -p:PublishSingleFile=true `
      -o dist/
}
