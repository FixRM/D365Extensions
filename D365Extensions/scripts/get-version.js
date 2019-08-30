'use strict';

try {
    //print package version
    const pack = require("./package.json");
    console.log(`package version ${pack.version}`);

    //set output variable
    const devops = require("azure-pipelines-task-lib");
    devops.setVariable("SemVersion", pack.version);
}
catch (e) {
    console.log(e);
}