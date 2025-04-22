NET=8.0

PROJECT_NAME=Redirector
SOLUTION_DIR=source
OUT_DIR=${SOLUTION_DIR}/${PROJECT_NAME}/bin/Release/net${NET}

all: build-release create-zip publish

build-release:
	@docker run \
		--rm \
		-v ./source:/var/app \
		-w /var/app \
		mcr.microsoft.com/dotnet/sdk:8.0 \
		dotnet build . -c Release

create-zip:
	@sudo rm -f ${OUT_DIR}/${PROJECT_NAME}
	@sudo rm -f ${OUT_DIR}/example.json
	@sudo rm -rf ${OUT_DIR}/emls
	@sudo rm -f ${OUT_DIR}/${PROJECT_NAME}.runtimeconfig.dev.json
	@sudo rm -f ${SOLUTION_DIR}/lambda.zip
	@zip ${SOLUTION_DIR}/lambda.zip ${OUT_DIR}/* -j

publish:
	@aws lambda update-function-code \
		--function-name redirect-emails \
		--zip-file fileb://${SOLUTION_DIR}/lambda.zip > result.json
	@cat result.json | grep State
	@cat result.json | grep LastUpdateStatus
	@rm result.json
