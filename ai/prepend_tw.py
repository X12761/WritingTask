import pandas as pd

df = pd.read_csv('indata.csv', sep=';')

# Keys before
df["D2"] = df["Down"].shift(-1)
df["U2"] = df["Up"].shift(-1)
df["D3"] = df["Down"].shift(-2)
df["U3"] = df["Up"].shift(-2)

# Calculate
df["D1U1"] = df["Up"] - df["Down"]          # U1 - D1
df["D1U2"] = df["U2"] - df["Down"]          # U2 - D1
df["D1D2"] = df["D2"] - df["Down"]          # D2 - D1
df["U1D2"] = df["D2"] - df["Up"]            # D2 - U1
df["U1U2"] = df["U2"] - df["Up"]            # U2 - U1
df["D1U3"] = df["U3"] - df["Down"]          # U3 - D1
df["D1D3"] = df["D3"] - df["Down"]          # D3 - D1


df["index"] = range(1, len(df) + 1)
df["delFreq"] = ((df["Key"] == "Delete") | (df["Key"] == "Back")).sum() / len(df)
result = df[["index","Key", "Down", "Up", "D1U1", "D1U2", "D1D2", "U1D2", "U1U2", "D1U3", "D1D3","delFreq"]]

# Store
result.to_csv("processed.csv", sep=";", index=False, encoding="utf-8")
