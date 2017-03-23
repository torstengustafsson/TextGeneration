import random

#The size of each 'unit' will be set by 'n'. With n=2 each unit contains two words etc.
def make_ngrams(text, n=2):
	# will be a dictionary with tuples as keys (n-gram sequences) and their next word as values
	pairs = dict()
	# strip text, break into words
	words = text.replace('\n', ' ').strip().split(" ")
	for i in range(len(words) - n):
		# tuple contains the words in the n-gram
		pair = tuple(words[i:i+n])
		if pair not in pairs:
			pairs[pair] = list()
		# add each instance of n-grams next word to value list, so long as it does not contain digits
		if words[i+n] not in pairs[pair] and len(''.join([i for i in words[i+n] if not i.isdigit()])) > 0:
			pairs[pair].append(words[i+n])
	return pairs

#Makes n-grams out of characters instead of words. Allows for more variation, but risk of nonsense words.
def make_ngrams_chars(text, n=2):
	pairs = dict()
	# only remove newline characters
	text = text.replace('\n', ' ')
	for i in range(len(text) - n):
		pair = tuple(text[i:i+n])
		if pair not in pairs:
			pairs[pair] = list()
		pairs[pair].append(text[i+n:i+n+2])
	return pairs

#takes an n-gram unit and finds a suitable next word from ngrams database
def generate_next_unit(unit, ngrams):
	# remove duplicates of the following word list
	following_words = list(set(ngrams[unit]))
	for val in ngrams.keys():
		# prevent direct recursive loops by checking equality of input unit and return value
		count = len(following_words)
		while count > 0:
			rand = random.randrange(0, len(following_words))
			if following_words[rand] == val[0] and unit != tuple(val):
				return tuple(val)
			count -= 1;
			rand = (rand + 1) % len(following_words)
	print("generated text ended with: " + str(ngrams[unit]))

#Generates a text based on a random starting unit, up to a maximum of 'count' units
def generate_text(ngrams, count=30, chars = False):
	units = list()
	for val in ngrams.keys():
		units.append(val)
	rand = random.randrange(0, len(units))
	# start from a random unit
	current_unit = units[rand]
	# 'text' will contain all units as a concatenated tuple that can be translated into pure text
	text = current_unit
	while count > 0:
		current_unit = generate_next_unit(current_unit, ngrams)
		if current_unit == None:
			break
		text += current_unit
		count -= 1
	res = ""
	if chars:
		res = ''.join(text)
	else:
		res = ' '.join(text)
	return res

#Removes units with less than 'count' following values
def filter_ngrams(count = 2):
	new_ngrams = dict()
	for val in ngrams.keys():
		if len(ngrams[val]) >= count:
			new_ngrams[val] = ngrams[val]
	return new_ngrams

#TODO: fix so it takes variable step and works with words instead of characters. Pretty useless now.
#EDIT: after finding a pattern, increase size by 1 and recheck. If patterns are still found, keep increasing by 1 until no more patterns found. then remove all duplicate patterns of max size.
#It should also apply weighs based on distance in text, and maybe amount of repeats. (if duplicate sequence is appropiately far away from the current we don't need to delete it)
def remove_patterns(text, step = 4):
	text = text.split(" ")
	newtext = text
	for i in range(len(text) - step):
		current = text[i:i+step]
		for j in range(i + step, len(text) - step + 1):
			if current == newtext[j:j+step]:
				newtext = newtext[:j] + newtext[j+step:]
	return " ".join(newtext)


####################
### MAIN PROGRAM ###
####################

books = ["lordofrings.txt", "twotowers.txt", "returnofking.txt"]
input_text = ""
for book in books:
	with open(book) as f:
		input_text += f.read()

ngrams = dict()
for i in range(2, 8):
	ngrams.update(make_ngrams(input_text, i))
ngrams = filter_ngrams(2)

output_text = generate_text(ngrams, 300)
#output_text = remove_patterns(output_text)

with open("text.txt", "w") as f:
	f.write(output_text)

print("\nGenerated text:")
print(output_text)
print("\nDatabase size: " + str(len(ngrams)))
