// azure-pipeline-updater.js

const regex = /semver:\s+(\d+\.\d+\.\d+)/;

module.exports.readVersion = function (contents) {
    return contents.match(regex)[1];
};

module.exports.writeVersion = function (contents, version) {
    return contents.replace(regex, `semver: ${version}`);
};