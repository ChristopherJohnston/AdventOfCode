import logging
logger = logging.getLogger(__name__)
logger.addHandler(logging.StreamHandler())
logger.setLevel(logging.INFO)

def is_valid(password):
    logger.debug('')

    str_password = str(password)

    # Check that the numbers always increment
    for i in range(0,5):
        if int(str_password[i]) > int(str_password[i+1]):
            return False

    # Check for repeating numbers
    repeating = []
    j = 0
    while j<5:
        r = 1
        for k in range(j+1,6):
            logger.debug('{0}, {1}'.format(str_password[j],str_password[k]))
            if str_password[j] != str_password[k]:
                break
            r+=1
        
        logger.debug('r={0}'.format(r))
        if r>0:
            repeating.append(r == 2)
        j+=r
     
    logger.debug(repeating)
    return any(repeating)

def password_combinations(lower, upper):
    combinations = []
    for i in range(lower, upper):
        if is_valid(i):
            combinations.append(i)
    
    return combinations          

def main():
    print (len(password_combinations(236491, 713787)))  # 1169, 757

    # logger.info(is_valid(111111)) # False
    # logger.info(is_valid(223450)) # False
    # logger.info(is_valid(123789)) # False
    # print(is_valid(112233)) # True
    # print(is_valid(123444)) # False
    # logger.info(is_valid(111122)) # True

if __name__ == "__main__":
    main()