class Consts:

    # General
    EXCEPTION_MESSAGE = "Program exited with exception"
    LOG_INFO = 'INFO: '
    LOG_ERROR = 'ERROR: '
    LOG_WARNING = 'WARNING: '
    LOG_VERBOSE = 'VERBOSE: '

    # Log Messages
    SERVICE_STARTING_AT = "Step 1/8: Process statrting {0}."
    CRON_ENABLED_AT = "Step 2/8: CronTime setup to {0}."
    READ_AND_PRODUCE_RUNNING = "Step 3/8: reading file"
    GENERATING_MESSAGES = "Step 4/8: messages generation"
    PROCESSING_MESSAGES = "Step 5/8: messages processing"
    MESSAGES_GENERATION_FINISHED = "Step 6/8: messages generation finished."
    SENT_MESSAGE_ERROR_DATA = "\nStep 7/8: Result: Sent message #{0}\nErrors: {1}\nData: {2}"
    MESSAGES_COUNTER = "Step 7/8: Result: Attempts to send {0} messages to Kafka Topic. Expected {1}"
    MESSAGES_ERROR_COUNTER = "Step 7/8: Result: Fail to sent {0} messages of {1}. Message errors: {2}."
    MESSAGES_SUCCESS_COUNTER = "Step 8/8: Result: Messages sent to Kafka Topic successfully: {0} from {1}"
    ALL_MESSAGES_SUCCESS = "Step 8/8: Result: All messages sent to Kafka Topic"
