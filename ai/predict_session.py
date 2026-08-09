import random
from time import sleep


sleep(2)
emot = {0:'A',1:'C',2:'H',3:'N',4:'S'}
s=open("ai/test.dat").read()

print(f"{emot[random.randrange(0,4)]} {s}")
