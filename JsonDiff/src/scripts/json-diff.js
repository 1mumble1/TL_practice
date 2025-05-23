const create = (oldObj, newObj) => {
    console.log('create');

    const keysOld = Object.keys(oldObj);
    const keysNew = Object.keys(newObj);
    const keysSet = new Set([...keysOld, ...keysNew]);
    const allKeys = [...keysSet.values()];

    const getType = (newValue, oldValue) => {
        if (oldValue === undefined) {
            return 'new';
        }

        if (newValue === undefined) {
            return 'deleted';
        }

        return (newValue === oldValue) ? 'unchanged' : 'changed';
    }

    const resultArray = allKeys.map((key) => {
        const newValue = newObj[key];
        const oldValue = oldObj[key];

        return {
            type: getType(newValue, oldValue),
            oldValue,
            newValue,
            key
        }
    });

    return resultArray.reduce((accumulator, {key, ...item}) => ({
        ...accumulator,
        [key]: { item }
    }
    ), {});
}

export const JsonDiff = { create };