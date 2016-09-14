import sys
from pylab import *

#sys.argv[1] = Nom du participant
#sys.argv[2] = Condition
#sys.argv[3] = Fichier de resultat du patient
#exemple : py drawGraphe.py Participant1 1 "Resultats\Participant1_11-25-2015 10-14-29 AM\Participant1.txt"

username = sys.argv[1]
condition = sys.argv[2]
filename = sys.argv[3]

f = open(filename, 'r')


valueArray = []
heightArray = []

def isInList(value, heightArray):
    for height in heightArray :
        if height == value :
            return True
    return False


for line in f:
    splittedLine = line.split('\t', 15)
    if splittedLine[1] == condition :
        height = float(splittedLine[3])
        answer = int(splittedLine[5])
        valueArray.append({'height': height, 'answer': answer})
        if isInList(height, heightArray) == False :
            heightArray.append(height)

averageArray = []

valueArray = sorted(valueArray, key=lambda value: value['height'])

height = valueArray[0]['height']
sum = 0
cpt = 0

for value in valueArray:
    if height != value['height'] :
        averageArray.append({'height': height, 'answer': sum / cpt})
        sum = 0
        cpt = 0
        height = value['height']
    sum = sum + value['answer']
    cpt = cpt + 1        

averageArray.append({'height': height, 'answer': sum / cpt})
	
x = []
y = []
for average in averageArray:
    x.append(average['height'])
    y.append(average['answer'])

	
plot(x, y, alpha=0.5)

title("Courbe de résultats du " + username + " pour la condition " + condition)
xlabel("Écartement des portes")
ylabel("Taux de réponses")
grid(True)
ylim([0,1])

splittedFile = filename.split('\\')

directory = ""

for i in range(0, len(splittedFile) -1) :
    directory = directory + splittedFile[i] + '\\'
	
savefig(directory + username + "_cond" + condition)

#show()	# Commenter si on ne veut pas faire l'affichage

valInf = averageArray[0]
valSup = 0

for average in averageArray :
    if average['answer'] < 0.5 :
        valInf = average
    else :
        valSup = average
        break
				
try :
    a = (valSup['answer'] - valInf['answer']) / (valSup['height'] - valInf['height'])
    b = valSup['answer'] - a * valSup['height']

    pse = (0.5 - b) / a

except :
    pse = 0


resultFile = open('Resultats/Resultat.txt', 'r+')

lines = resultFile.readlines()

resultFile.close()
resultFile = open('Resultats/Resultat.txt', 'w')

for line in lines :
    parameters = line.replace("\n", "").split('\t')
    if parameters[0] == username :
        res = ""
        for index in range(0, len(line.split('\t'))) :
            if index == 14 + int(condition) : 
                res = res + str(pse) + '\t'
            else :
                res = res + parameters[index] + '\t'
        line = res + '\n'
    resultFile.write(line)
	
resultFile.close()