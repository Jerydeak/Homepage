window.NoteGenerator = (function () {
    // private
    function copy(list) {
        var ret = [];
        for (let i = 0; i < list.length; i++) ret[i] = list[i];
        // console.log('copy', ret);
        return ret;
    }
    function sum(list) {
        var val = 0;
        for (let i = 0; i < list.length; i++) val += list[i];
        // console.log('sum', val);
        return val;
    }
    function normalize(list) {
        var list = copy(list);
        var sumval = sum(list);
        if (sumval == 0) return list;
        for (let i = 0; i < list.length; i++) list[i] /= sumval;
        return list;
    }
    // public
    return {
        randomMatrix: function (width, range, density) {
            if (typeof (density) != 'number' || density < 0 || density > 1) {
                console.error("density should be a number within [0, 1], but " + density + " found.");
                return;
            }
            ret = [];
            for (let i = 0; i < width; i++) {
                temp = []
                for (let j = 0; j < range; j++) {
                    temp[j] = (Math.random() < density) ? 1 : 0;
                }
                ret[i] = temp;
            }
            return ret;
        },
        note12Matrix: function (width, getExpect, originDistribution) {
            var distribution = normalize(originDistribution);
            var impactWeight = [3, 1, 3, 5, 5, 10, 3, 10, 5, 5, 3, 1];
            var ret = [];
            for (let i = 0; i < width; i++) {
                var expect = getExpect(i);
                console.log(distribution);
                var temp = []
                var impact = [];
                for (let j = 0; j < 12; j++) impact.push(1);
                for (let j = 0; j < 12; j++) {
                    var r = Math.random();
                    if (r < expect * distribution[j]) {
                        temp[j] = 1;
                        for (let k = 0; k < 12; k++) impact[(j + k) % 12] += impactWeight[k];
                    }
                    else temp[j] = 0;
                }
                ret[i] = temp;
                for (let j = 0; j < 12; j++) distribution[j] *= impact[j];
                distribution = normalize(distribution);
            }
            return ret;
        }
    }
})();