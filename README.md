# Projeto---Game-JAM

# Instalação do Unity e Unity Hub [Utilize o Powershell como administrador, pré-requisitos: versão do windows >= Windows 10 1709 (build 16299)]

```powershell
Set-ExecutionPolicy Bypass -Scope Process -Force; [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; iex ((New-Object System.Net.WebClient).DownloadString('https://community.chocolatey.org/install.ps1'))
Add-AppxPackage -RegisterByFamilyName -MainPackage Microsoft.DesktopAppInstaller_8wekyb3d8bbwe
choco install unity --version=6000.0.40 --yes
winget install --id Unity.UnityHub --accept-source-agreements
```

# Clonar o projeto na máquina

```bash
mkdir projeto_game_jam
cd projeto_game_jam
git clone https://github.com/2000Paulo/GameNerith.git .
git checkout luan
```

# Guia para Antes & Durante o commit

```bash
git pull origin luan
git add [coloque aqui o nome dos arquivos modificados ou criados]
git commit -m [A sua mensagem de commit descrevendo o que foi feito]
git push origin luan
```