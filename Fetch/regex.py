import pyodbc
# import json
import re
import os

path = "sql/"
outPath = "output.sql"

# def connect():    
#     with open('server.json', 'r') as f:
#         mssql = json.load(f)
#     server = mssql['server'] 
#     database = mssql['database']
#     username = mssql['username']
#     password = mssql['password']
#     cnxn = pyodbc.connect('DRIVER={ODBC Driver 17 for SQL Server};SERVER='+server+';DATABASE='+database+';UID='+username+';PWD='+ password)
#     cursor = cnxn.cursor()
#     return cursor
    
    # windows login
    # cnxn = pyodbc.connect(Trusted_Connection = 'yes', driver = '{SQL Server}',server = '(local)' , database = 'iEat')
    # cursor = cnxn.cursor() 
    # cnxn.close()

def main():
    # cursor = connect()

    level2Query = """
EXEC sys.sp_addextendedproperty 
    @name = '[name]', @value = '[value]',             
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = '[level1type]', @level1name = '[level1name]',
    @level2type = '[level2type]', @level2name = '[level2name]'
        """

    level1Query = """
EXEC sys.sp_addextendedproperty 
    @name = '[name]', @value = '[value]',             
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = '[level1type]', @level1name = '[level1name]'
        """
    
    query = ""

    for folder in os.scandir(path):
        if (folder.name == ".DS_Store"): continue
        folderPath = path + folder.name
        # print(folderPath)

        # table
        with open(folderPath, "r", encoding='utf8') as f:
            data = f.read().splitlines()
            firstLine = True
            secRule = ""
            # funcOutRule = ""
            tableType = []
            tableName = ""
            columnName = ""

            for d in data:
                # 判斷是不是第一行 (create)
                if firstLine:
                    rule = ""
                    key = d.lower()
                    if "create view" in key:
                        rule = "(?i)CREATE VIEW (\S+) (?i)as\s*--\s*(.+)"
                        # secRule = "\s*(\S+)\s*--\s*(.+)"
                        secRule = "\S\.\S+ as (\S+) \s*--\s*(.+)"
                        tableType = ["VIEW", "COLUMN"]
                    elif "create table" in key:
                        rule = "(?i)CREATE TABLE (\S+)\s\S\s*--\s*(.+)"
                        secRule = "\s*(\S+)\s\S+\s*--\s*(\S+)"
                        tableType = ["TABLE", "COLUMN"]
                    elif "create function" in key:
                        rule = "(?i)CREATE FUNCTION (\S+)\s*--\s*(.+)"
                        secRule = "\s*(\S+)\s\S+\s*--\s*(.+)"
                        # funcOutRule = "(?i)RETURNS\s\S+\s*--\s*(.+)"
                        tableType = ["FUNCTION", "PARAMETER"]
                    elif "create procedure" in key:
                        rule = "(?i)CREATE PROCEDURE (\S+)\s*--\s*(.+)"
                        secRule = "\s*(\S+)\s\S+\s*--\s*(.+)"
                        tableType = ["PROCEDURE", "PARAMETER"]

                    # table/view/function/procedure name
                    try:
                        pattern = re.compile(rule)
                        result = pattern.search(d)
                        tableName = result.group(1)
                        DesName = "DS_" + tableName + "_"
                        query += level1Query.replace("[name]", DesName).replace("[value]", result.group(2)).replace("[level1type]", tableType[0]).replace("[level1name]", tableName) + "\n"
                        # val = (DesName, result.group(2), tableType[0], tableName)
                        # cursor.execute(level1Query, val)
                        # cursor.commit()
                        # print("Table Remark: " + tableName, result.group(2))
                        firstLine = False
                    except:
                        # print("no find")
                        continue
                else:
                    if "begin" in d: break
                    # table/view/function/procedure column
                    try:
                        pattern = re.compile(secRule)
                        result = pattern.search(d)
                        columnName = result.group(1)
                        if columnName.find(","):
                            columnName = columnName.replace(",", "")
                        DesName = "DS_" + tableName + "_" + columnName
                        query += level2Query.replace("[name]", DesName).replace("[value]", result.group(2)).replace("[level1type]", tableType[0]).replace("[level1name]", tableName).replace("[level2type]", tableType[1]).replace("[level2name]", columnName) + "\n"
                        # val = (DesName, result.group(2), tableType[0], tableName, tableType[1], columnName)
                        # cursor.execute(level2Query, val)
                        # cursor.commit()
                        # print("Column Remark: " + columnName, result.group(2))
                    except:
                        # print("no find")
                        continue
                
        fw = open(outPath, "w", encoding='utf-8')
        fw.write(query)


if __name__ == "__main__":
    main()




# Regex Expression

# table

# (?i)CREATE TABLE (\S+)\s\S\s*--\s*(.+)
# \s*(\S+)\s\S+\s*--\s*(\S+)

# view

# (?i)CREATE VIEW (\S+) (?i)as\s*--\s*(.+)
# \s*(\S+)\s*--\s*(.+)


# function

# (?i)CREATE FUNCTION (\S+)\s*--\s*(.+)
# \s*(\S+)\s\S+\s*--\s*(.+)
# (?i)RETURNS\s\S+\s*--\s*(.+)


# procedure

# (?i)CREATE PROCEDURE (\S+)\s*--\s*(.+)
# \s*(\S+)\s\S+\s*--\s*(.+)
