import logging

# DEBUG: Detailed information, typically of interest only when diagnosing problems.
# INFO: Confirmation that things are working as expected.
# WARNING: An indication that something unexpected happened, or indicative of some problem in the near future (e.g. ‘disk space low’). The software is still working as expected.
# ERROR: Due to a more serious problem, the software has not been able to perform some function.
# CRITICAL: A serious error, indicating that the program itself may be unable to continue running.

logging.basicConfig(filename='test.txt',level=logging.DEBUG,
                    format='%(asctime)s:%(levelname)s:%(message)s')

def add(a, b):
    return a+b
def sub(a, b):
    return a-b
def mul(a, b):
    return a-b
def div(a, b):
    return a/b

num1 = 10
num2 = 5

addResult = add(10, 5)
logging.debug('Add: {} + {} = {}'.format(num1, num2, addResult));

subtractResult = sub(10, 5)
logging.error('Subtract: {} + {} = {}'.format(num1, num2, subtractResult));

mutltiplyResult = mul(10,5)
logging.debug('Multiply: {} + {} = {}'.format(num1, num2, mutltiplyResult));

divResult = div(10,5)
logging.debug('Multiply: {} + {} = {}'.format(num1, num2, divResult));