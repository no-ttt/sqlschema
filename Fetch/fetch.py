import pyodbc
import json
import os

path = "sql/"

def connect():    
    with open('server.json', 'r') as f:
        mssql = json.load(f)
    server = mssql['server'] 
    database = mssql['database']
    username = mssql['username']
    password = mssql['password']
    cnxn = pyodbc.connect('DRIVER={ODBC Driver 17 for SQL Server};SERVER='+server+';DATABASE='+database+';UID='+username+';PWD='+ password)
    cursor = cnxn.cursor()
    return cursor
    
    # windows login
    # cnxn = pyodbc.connect(Trusted_Connection = 'yes', driver = '{SQL Server}',server = '(local)' , database = 'iEat')
    # cursor = cnxn.cursor() 
    # cnxn.close()


def main():
    cursor = connect()

    level2Query = """
            EXEC sys.sp_addextendedproperty 
                @name = ?, @value = ?,             
                @level0type = N'SCHEMA', @level0name = N'dbo', 
                @level1type = ?, @level1name = ?,
                @level2type = ?, @level2name = ?
        """

    level1Query = """
            EXEC sys.sp_addextendedproperty 
                @name = ?, @value = ?,             
                @level0type = N'SCHEMA', @level0name = N'dbo', 
                @level1type = ?, @level1name = ?
        """

    tableName = ""
    type = []

    for folder in os.scandir(path):
        if (folder.name == ".DS_Store"): continue
        folderPath = path + folder.name
        print(folderPath)
        for file in os.scandir(folderPath):
            print(file.name)
            idx = 0
            tableName = file.name.split(".sql")[0]
            with open(file, "r", encoding='utf-8') as f:
                data = f.read().splitlines()
                for d in data:
                    if idx == 0: 
                        if "CREATE PROCEDURE" in d:
                            type = ["PROCEDURE", "PARAMETER"]
                        if "CREATE FUNCTION" in d:
                            type = ["FUNCTION", "PARAMETER"]
                        if "CREATE VIEW" in d:
                            type = ["VIEW", "COLUMN"]
                        if "CREATE TABLE" in d:
                            type = ["TABLE", "COLUMN"]
                        
                        # level 0
                        if "--" in d:
                            tableRemark = d.split('-- ')[-1] 
                            DesName = "DS_" + tableName + "_"
                            val = (DesName, tableRemark, type[0], tableName)
                            cursor.execute(level1Query, val)
                            cursor.commit()
                            print(val)
                            
                        idx += 1
                        continue

                    # level 1
                    if "--" in d:
                        arr = d.split()
                        # arr = [x for x in arr if x != '']
                        columnName = columnDesName = arr[0]
                        if (folder.name == "Procedure" or folder.name == "Function"):
                            columnDesName = columnName.replace("@", "")
                        if (folder.name == "View"):
                            columnDesName = columnName.replace(",", "")
                            columnName = columnName.replace(",", "")

                        columnRemark = d.split('-- ')[-1]
                        DesName = "DS_" + tableName + "_" + columnDesName
                        val = (DesName, columnRemark, type[0], tableName, type[1], columnName)
                        cursor.execute(level2Query, val)
                        cursor.commit()
                        print(val)
                    

if __name__ == "__main__":
    main()


