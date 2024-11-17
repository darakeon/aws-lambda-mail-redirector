NET=8.0

PROJECT_NAME=Redirector
SOLUTION_DIR=source
OUT_DIR=${SOLUTION_DIR}/${PROJECT_NAME}/bin/Release/net${NET}/

create-zip:
	@rm -f ${OUT_DIR}${PROJECT_NAME}.exe
	@rm -f ${OUT_DIR}example.json
	@rm -rf ${OUT_DIR}emls
	@rm -f ${OUT_DIR}${PROJECT_NAME}.runtimeconfig.dev.json
	@zip ${SOLUTION_DIR}/lambda.zip ${OUT_DIR}* -j 
