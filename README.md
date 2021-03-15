# nqrgen


![demo#1](img/WindowsTerminal_Ob0Jvv7Kj1.png)

## usage

```powershell
cat .\hello.txt | dotnet run -c Release --
cat .\hello.txt | dotnet run -c Release -- -r File -d TestResults -s 300
cat .\hello.txt | dotnet run -c Release -- -r File -d TestResults -s 300 -m 1 -c High

```