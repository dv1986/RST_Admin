
    export function termExistsInObject(term: string, item: any): boolean {
        term = term.toLowerCase();
        let found = false;
        var keys = Object.keys(item)
        keys.forEach(element => {

            if (!found) {
                found = item[element].toString().toLowerCase().indexOf(term) !== -1;
            }
        });
        return found;
    }

        