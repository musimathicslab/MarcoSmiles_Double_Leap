import os
import numpy as np
from sklearn.metrics import precision_recall_fscore_support as score
from sklearn.preprocessing import RobustScaler, StandardScaler, MinMaxScaler
from sklearn.preprocessing import LabelEncoder
from sklearn.model_selection import GridSearchCV
from numpy import mean
from sklearn.metrics import accuracy_score
from sklearn.model_selection import train_test_split
from sklearn.neural_network import MLPClassifier
import pickle
from sklearn.ensemble import ExtraTreesClassifier
import matplotlib.pyplot as plt

import pandas as pd
from _datetime import datetime


with open('learning_time.txt', 'w') as lt:
    now = datetime.now()
    # dd/mm/YY H:M:S
    dt_string = now.strftime("%d/%m/%Y %H:%M:%S")
    lt.write(str("Data Inizio Learning: " + dt_string + "\n"))


def feature_importance(X, y):
    '''
    Show a plot of features importance.
    Each feature is identified by the index it occupies in the sample array.
    '''

    # Build a forest and compute the impurity-based feature importance
    forest = ExtraTreesClassifier(n_estimators=250,
                                  random_state=0)

    forest.fit(X, y)
    importances = forest.feature_importances_
    std = np.std([tree.feature_importances_ for tree in forest.estimators_],
                 axis=0)
    indices = np.argsort(importances)[::-1]

    # Print the feature ranking
    print("Feature ranking:")

    for f in range(X.shape[1]):
        print("%d. feature %d (%f)" %
              (f + 1, indices[f], importances[indices[f]]))

    # Plot the impurity-based feature importances of the forest
    plt.figure()
    plt.title("Feature importance.")
    plt.bar(range(X.shape[1]), importances[indices],
            color="r", yerr=std[indices], align="center")
    plt.xticks(range(X.shape[1]), indices)
    plt.xlim([-1, X.shape[1]])
    # plt.show()
    # plt.savefig('foo.png')
    plt.savefig('foo.pdf')
    # plt.savefig('foo.png', bbox_inches='tight')


print("\n\n\n\n\n#####\t\t IGNORE ALL THE FOLLOWING MESSAGES, EVERYTHING IS WORKING FINE. \t\t#####")
print("#####\t\t IGNORE ALL THE FOLLOWING MESSAGES, EVERYTHING IS WORKING FINE. \t\t#####")
print("#####\t\t IGNORE ALL THE FOLLOWING MESSAGES, EVERYTHING IS WORKING FINE. \t\t#####")
print("#####\t\t IGNORE ALL THE FOLLOWING MESSAGES, EVERYTHING IS WORKING FINE. \t\t#####")
print("#####\t\t IGNORE ALL THE FOLLOWING MESSAGES, EVERYTHING IS WORKING FINE. \t\t#####")
print("#####\t\t IGNORE ALL THE FOLLOWING MESSAGES, EVERYTHING IS WORKING FINE. \t\t#####")
print("#####\t\t IGNORE ALL THE FOLLOWING MESSAGES, EVERYTHING IS WORKING FINE. \t\t#####")
print("#####\t\t IGNORE ALL THE FOLLOWING MESSAGES, EVERYTHING IS WORKING FINE. \t\t#####")
print("#####\t\t IGNORE ALL THE FOLLOWING MESSAGES, EVERYTHING IS WORKING FINE. \t\t#####")
print("#####\t\t IGNORE ALL THE FOLLOWING MESSAGES, EVERYTHING IS WORKING FINE. \t\t#####\n\n")


def grid(cl, classifier, param_grid, n_folds, t_s_D, tLab_downsampled):

    # Change the path where the various validation experiments are saved with k-fold cross-validation
    with open(cl+".out.txt", "w") as f:

        # Chooses how many CPU cores to use, only 5/6 of all available cores will be used
        quotient = 5/6
        n_core = int(quotient * os.cpu_count())

        estimator = GridSearchCV(
            classifier, cv=n_folds, param_grid=param_grid, n_jobs=n_core, verbose=1, scoring='f1_weighted')
        estimator.fit(t_s_D, tLab_downsampled)
        means = estimator.cv_results_['mean_test_score']
        stds = estimator.cv_results_['std_test_score']
        best = []
        max = 0
        for meana, std, params in zip(means, stds, estimator.cv_results_['params']):
            if meana > max:
                max = meana
                best = params
            print("%0.3f (+/-%0.03f) for %r" % (meana, std * 2, params))
            f.write("%0.3f (+/-%0.03f) for %r" % (meana, std * 2, params))
            f.write("\n")
        print()
    return best


'''
The dataset must have this form. Preferably an np.array
Remember that np.array cannot be declared empty and filled in as you go,
but must be declared e.g. a row of X elements where X is input np.zeros(18), i.e. 18 elements all 0
and then queue the samples/elements per line with concatenate or append - find syntax - 
remember to delete first row with all 0. 


X example (it is an array) each row is a sample
__________________________
| 3 5 7 9 12 9 1 0.5 0.1 0| element 1 of the dataset
| 1 2 5 9 12 8 1 0.6 0.2 1| element 2 of the dataset
| ...                     |
| 1 3 7 8 11 8 2 0.3 0.2 0| dataset element n
___________________________


y example (is a column array) defines the class/label/note of each element/sample
___
|1| element 1 class
|2| element 2 class
|1| element 3 class
|0| element 4 class
|1| element class 5
|3| element class 6
|...|
|0| element class n

'''


dataset_array = pd.read_csv('marcosmiles_dataset.csv', sep=',', header=None)
x = dataset_array.copy()
y = dataset_array.copy()

#
# -------------------------- TWO DEVICES DATASET -------------------------- #
#
#

# Create dataset
xcols = [36]
ycols = [index for index in range(0,36)]

x.drop(x.columns[xcols], axis=1, inplace=True)
y.drop(y.columns[ycols], axis=1, inplace=True)

l_enc = LabelEncoder()
i_enc = l_enc.fit_transform(y.values.ravel())
i_enc = i_enc.reshape(len(i_enc))

# 1. Split the dataset in a stratified manner into a part for training (80%) and a part for testing (20%)
training_set_data, test_set_data, training_set_labels, test_set_labels = train_test_split(x, i_enc, test_size=0.2, stratify=i_enc)
#training_set_data, test_set_data, training_set_labels, test_set_labels = train_test_split(x, y, test_size=0.2, stratify=y)


# Compute Max and min values for the 2 hand dataset
min_values = []
max_values = []

for col in ycols:
    min_values.append(min(training_set_data[col]))
    max_values.append(max(training_set_data[col]))

with open('min&max_values_dataset_out.txt', 'w') as f:
    for item_min in min_values:
        f.write(str(item_min) + " ")
    f.write("\n")
    for item_max in max_values:
        f.write(str(item_max) + " ")


# 2 Normalize data
scaler = MinMaxScaler()
train_scaled_D = scaler.fit_transform(training_set_data)
test_scaled_D = scaler.transform(test_set_data)

# Feature importance
feature_importance(train_scaled_D, training_set_labels)

# validation con k-fold cross-validation
n_folds = 10


# Classifier
classificatore = MLPClassifier()
# 3 parameters to be tested, replace hidden layer number by input size, replace max_iter by dataset size
# ['tanh', 'relu']

pg = {'activation': ['relu'], 'learning_rate': ['invscaling', 'adaptive', 'constant'],
      'solver': ['adam', 'lbfgs', 'sgd'],
      'learning_rate_init': [0.01, 0.05, 0.1, 0.15, 0.2], 'hidden_layer_sizes': [3, 5, 10, 15, 17],
      'max_iter': [2500, 5000, 10000]}


#pg = {'activation': ['relu','tanh'], 'learning_rate': ['invscaling'],
   #  'solver': ['adam'],
    # 'learning_rate_init': [0.01, 0.05, 0.1, 0.15, 0.2], 'hidden_layer_sizes': [3, 5, 10, 15, 17],
   #  'max_iter': [2500, 5000, 10000]}


#bestMLPparam = grid("marcosmiles_dataset.csv", classificatore,pg, n_folds, train_scaled_D, training_set_labels.values.ravel())
bestMLPparam = grid("marcosmiles_dataset.csv", classificatore,pg, n_folds, train_scaled_D, training_set_labels)
print("Best parameters for MLP:")
print(bestMLPparam)

classificatore = MLPClassifier(activation=bestMLPparam['activation'], learning_rate=bestMLPparam['learning_rate'], solver=bestMLPparam['solver'],
                               learning_rate_init=bestMLPparam['learning_rate_init'], max_iter=bestMLPparam['max_iter'],
                               hidden_layer_sizes=bestMLPparam['hidden_layer_sizes'])
#classificatore.fit(train_scaled_D, training_set_labels.values.ravel())
classificatore.fit(train_scaled_D, training_set_labels)

n_notes = len(set(training_set_labels))
l_original = l_enc.inverse_transform(np.arange(n_notes))
with open('lbl_notes.txt', 'w') as fw:       # new line \n identifies the start of a new layer
    for note in l_original:
        print(str(note))
        print("\n")
        fw.write(str(note))
        fw.write("\n")

y_pred = classificatore.predict(test_scaled_D)
precision, recall, fscore, support = score(test_set_labels, y_pred)
accuracy = accuracy_score(test_set_labels, y_pred)
print("MLP")
print("Accuracy")
print(accuracy)
print('precision: {}'.format(precision))
print('recall: {}'.format(recall))
print('fscore: {}'.format(fscore))
print("Weight and Bias")
# The i-th element in the list represents the weight W matrix corresponding to layer i.
print(classificatore.coefs_)
# The i-th element in the list represents the bias B vector corresponding to layer i + 1.
print(classificatore.intercepts_)

# Save Model
with open ('model.pkl', 'wb') as ml_file:
    pickle.dump(classificatore, ml_file)

with open('precision_out.txt', 'w') as aw:
    aw.write("Accuracy: ")
    aw.write(str(accuracy))
    aw.write("\n")
    aw.write("Precision: ")
    aw.write(str(precision))
    aw.write("\n")
    aw.write("Recall: ")
    aw.write(str(recall))
    aw.write("\n")
    aw.write("fscore: ")
    aw.write(str(fscore))
    aw.write("\n")


with open('weights_out.txt', 'w') as fw:       # new line \n identifies the start of a new layer
    for w_layer in classificatore.coefs_:
        print(str(w_layer))
        print("\n")
        fw.write(str(w_layer))
        fw.write("\n")

# # new line \n identifies the start of a new layer, start from i+1
with open('bias_out.txt', 'w') as fb:
    for b_layer in classificatore.intercepts_:
        fb.write(str(b_layer))
        fb.write("\n")


with open('learning_time.txt', 'a+') as lt:
    now = datetime.now()
    # dd/mm/YY H:M:S
    dt_string = now.strftime("%d/%m/%Y %H:%M:%S")
    lt.write(str("Data Fine Learning: " + dt_string + "\n"))


#
# -------------------------- ONE DEVICE DATASET -------------------------- #
#
#

x_1H=dataset_array.copy()
y_1H=dataset_array.copy()

# Create dataset
xcols_1H=[index for index in range(18,37)]
ycols_1H = [index for index in range(0,36)]

x_1H.drop(x_1H[xcols_1H],axis=1,inplace=True)
# the labels are the same
y_1H=y


# 1. Split the dataset in a stratified manner into a part for training (80%) and a part for testing (20%)
training_set_data_1H, test_set_data_1H, training_set_labels_1H, test_set_labels_1H = train_test_split(x_1H, i_enc, test_size=0.2, stratify=i_enc)
#training_set_data, test_set_data, training_set_labels, test_set_labels = train_test_split(x, y, test_size=0.2, stratify=y)

# Compute Max and min values for the 1 hand dataset
min_values_1H = []
max_values_1H = []

for col in ycols:
    min_values_1H.append(min(training_set_data_1H[col]))
    max_values_1H.append(max(training_set_data_1H[col]))

with open('min&max_values_dataset_out_1H.txt', 'w') as f:
    for item_min in min_values_1H:
        f.write(str(item_min) + " ")
    f.write("\n")
    for item_max in max_values_1H:
        f.write(str(item_max) + " ")


# 2 Normalize data
train_scaled_D_1H = scaler.fit_transform(training_set_data_1H)
test_scaled_D_1H = scaler.transform(test_set_data_1H)

# Feature importance
feature_importance(train_scaled_D_1H, training_set_labels_1H)


# Validation with k-fold cross-validation
n_folds = 10


# Classifier
classificatore_1H = MLPClassifier()
# 3 parameters to be tested, replace hidden layer number by input size, replace max_iter by dataset size
# ['tanh', 'relu']

pg = {'activation': ['relu'], 'learning_rate': ['invscaling', 'adaptive', 'constant'],
      'solver': ['adam', 'lbfgs', 'sgd'],
      'learning_rate_init': [0.01, 0.05, 0.1, 0.15, 0.2], 'hidden_layer_sizes': [3, 5, 10, 15, 17],
      'max_iter': [2500, 5000, 10000]}


#pg = {'activation': ['relu','tanh'], 'learning_rate': ['invscaling'],
   #  'solver': ['adam'],
    # 'learning_rate_init': [0.01, 0.05, 0.1, 0.15, 0.2], 'hidden_layer_sizes': [3, 5, 10, 15, 17],
   #  'max_iter': [2500, 5000, 10000]}


#bestMLPparam = grid("marcosmiles_dataset.csv", classificatore,pg, n_folds, train_scaled_D, training_set_labels.values.ravel())
bestMLPparam = grid("marcosmiles_dataset.csv", classificatore_1H,pg, n_folds, train_scaled_D_1H, training_set_labels_1H)
print("Best parameters for MLP are:")
print(bestMLPparam)

classificatore_1H = MLPClassifier(activation=bestMLPparam['activation'], learning_rate=bestMLPparam['learning_rate'], solver=bestMLPparam['solver'],
                               learning_rate_init=bestMLPparam['learning_rate_init'], max_iter=bestMLPparam['max_iter'],
                               hidden_layer_sizes=bestMLPparam['hidden_layer_sizes'])
#classificatore.fit(train_scaled_D, training_set_labels.values.ravel())
classificatore_1H.fit(train_scaled_D_1H, training_set_labels_1H)


y_pred_1H = classificatore_1H.predict(test_scaled_D_1H)
precision, recall, fscore, support = score(test_set_labels_1H, y_pred_1H)
accuracy = accuracy_score(test_set_labels_1H, y_pred_1H)
print("=========================== RESULTS FOR 1 HAND ============================")
print("MLP")
print("Accuracy")
print(accuracy)
print('precision: {}'.format(precision))
print('recall: {}'.format(recall))
print('fscore: {}'.format(fscore))
print("Weight and Bias")
# The i-th element in the list represents the weight W matrix corresponding to layer i.
print(classificatore_1H.coefs_)
# The i-th element in the list represents the bias B vector corresponding to layer i + 1.
print(classificatore_1H.intercepts_)

# Save model
with open ('model_1H.pkl', 'wb') as ml_file:
    pickle.dump(classificatore_1H, ml_file)

with open('precision_out_1H.txt', 'w') as aw:
    aw.write("Accuracy: ")
    aw.write(str(accuracy))
    aw.write("\n")
    aw.write("Precision: ")
    aw.write(str(precision))
    aw.write("\n")
    aw.write("Recall: ")
    aw.write(str(recall))
    aw.write("\n")
    aw.write("fscore: ")
    aw.write(str(fscore))
    aw.write("\n")


with open('weights_out_1H.txt', 'w') as fw:       # new line \n identifies the start of a new layer
    for w_layer in classificatore_1H.coefs_:
        print(str(w_layer))
        print("\n")
        fw.write(str(w_layer))
        fw.write("\n")

# new line \n identifies the start of a new layer, start from i+1
with open('bias_out_1H.txt', 'w') as fb:
    for b_layer in classificatore_1H.intercepts_:
        fb.write(str(b_layer))
        fb.write("\n")

with open('learning_time.txt', 'a+') as lt:
    now = datetime.now()
    # dd/mm/YY H:M:S
    dt_string = now.strftime("%d/%m/%Y %H:%M:%S")
    lt.write(str("Data Fine Learning: " + dt_string + "\n"))

