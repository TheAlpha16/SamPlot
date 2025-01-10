import os.path as pth

files = [
    "Melatonin_150 mM_100 Micrwatt_CA_ext.txt",
    "DMSO_100 Micrwatt_CA_ext.txt",
    "Melatonin_1 mM_100 Micrwatt_CA_ext.txt",
    "Melatonin_250 mM_100 Micrwatt_CA_red.txt",
    "Melatonin_50 mM_100 Micrwatt_CA_ext.txt",
]

for filename in files:
    rows = []

    with open(pth.join(".", "raw", filename), "r") as f:
        lines = list(map(lambda x: x.strip(), f.read().strip().split("\n")))
        for line in lines:
            x, y1, y2 = map(float, line.split("\t"))
            rows.append([x, y1])
        
    with open(pth.join(".", "cleaned", f"{filename.strip('.tx')}.csv"), "w") as f:
        f.write("x,y\n")
        f.write("\n".join(map(lambda x: f"{round(x[0], 5)},{round(x[1], 5)}", rows)) + "\n")