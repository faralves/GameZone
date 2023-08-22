#aguardando 90 segundos para aguardar o provisionamento e start do banco
echo "Aguardando 90 segundos..."
sleep 90s
echo "Continuando com os passos após o provisionamento e start do banco"
#rodar o comando para criar o banco
/opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U SA -P "Mudar123intrA" -i criacao-banco-docker.sql