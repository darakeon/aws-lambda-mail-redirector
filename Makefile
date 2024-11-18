NET=8.0

PROJECT_NAME=Redirector
SOLUTION_DIR=source
OUT_DIR=${SOLUTION_DIR}/${PROJECT_NAME}/bin/Release/net${NET}

all: build-release create-zip

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
